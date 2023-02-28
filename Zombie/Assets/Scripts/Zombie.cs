using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오기

// 좀비 AI 구현
public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어

    private LivingEntity targetEntity; // 추적 대상
    private NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피격 시 재생할 소리

    private Animator zombieAnimator; // 애니메이터 컴포넌트
    private AudioSource zombieAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer zombieRenderer; // 렌더러 컴포넌트

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    // Awake() 는 Start() 와 유사하게 초기 1회 자동 실행되는 유니티 이벤트 메서드
    // 다만, Start() 보다 실행시점이 한 프레임 더 빠름
    private void Awake() {
        // 좀비 게임 오브젝트로부터 필요한 컴포넌트들을 가져옴
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

        // 좀비 게임 오브젝트의 자식 게임 오브젝트에 할당된 컴포넌트인 Skinned Mesh Renderer 컴포넌트를 가져옴
        zombieRenderer = GetComponentInChildren<Renderer>();
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    // Zombie 스크립트 컴포넌트 내부에서 사용하는 메서드가 아니라,
    // '좀비 생성기' 에서 생성된 좀비의 능력치를 설정할 때 실행하기 위해 public 으로 뚫어놓은 메서드
    // 인자로 전달받는 zombieData 는 스크립터블 오브젝트 에셋 형태로 전달받는 데이터 컨테이너
    public void Setup(ZombieData zombieData) {
        // 체력 설정
        startingHealth = zombieData.health;
        health = zombieData.health;

        // 공격력 설정
        damage = zombieData.damage;

        // 내비메시 에이전트(생성된 좀비)의 이동 속도 설정
        navMeshAgent.speed = zombieData.speed;

        // 렌더러 컴포넌트가 사용중인 머티리얼의 컬러 변경 -> 생성된 좀비의 외형 색이 변함
        zombieRenderer.material.color = zombieData.skinColor;
    }

    private void Start() {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update() {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath() {
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            // 추척 대상 존재 여부에 따라 처리가 달라짐
            if (hasTarget)
            {
                // 추적 대상이 존재하면? 1. 네비메시 에이전트의 이동 재시작 2. 네비메시 에이전트의 목표 위치 재설정을 통한 이동경로 재설정
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                // 추적 대상이 존재하지 않는다면? 1. 네비메시 에이전트 이동 중지 2. 좀비 게임 오브젝트의 일정 반경 내에 새로운 추적대상을 찾아 갱신
                // 네비메시 에이전트 이동 중단 처리
                navMeshAgent.isStopped = true;

                // 좀비 게임 오브젝트의 전역공간 위치를 중심으로 20유닛 반지름을 갖는 가상의 구를 그렸을 때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, 모든 콜라이더와 교차여부를 검사하면 성능 낭비니까, WhatIsTarget 레이어 마스크에 해당하는 레이어를 갖는 콜라이더만 교차여부를 검사해서 가져옴
                // WahtIsTarget 에는 나중에 인스펙터 창을 통해 Player 레이어를 할당할 것이므로, 사실상 구와 겹치는 플레이어 게임 오브젝트의 콜라이더들만 가져온다고 보면 됨.
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                // 겹치는 콜라이더들을 순회하면서, 살아 있는 LivingEntity 컴포넌트를 찾음
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 현재 콜라이더를 갖고 있는 게임 오브젝트로부터 LivingEntity 컴포넌트를 찾아서 가져오기
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // 가져온 LivingEntity 컴포넌트가 존재하고, 그 LivingEntity 의 사망상태가 살아있다면 추적대상을 해당 LivingEntity 로 갱신
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // 추적대상 갱신
                        targetEntity = livingEntity;

                        // 추적대상 갱신을 완료했다면, for 문 루프를 중단하고 나머지 콜라이더들은 무시함
                        break;
                    }
                }
            }

            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을 때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        // 사망하지 않은 상태에서만 피격 효과 재생
        if (!dead)
        {
            // 공격받은 지점과 방향으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint; // 공격받은 지점을 파티클 게임 오브젝트의 위치로 지정
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); // 파티클 게임 오브젝트가 공격받은 방향을 바라보도록 회전값 변경
            hitEffect.Play(); // 파티클 시스템 컴포넌트 재생

            // 파티클 효과음 재생
            zombieAudioPlayer.PlayOneShot(hitSound);
        }

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 좀비 게임 오브젝트의 내비메시 에이전트의 이동을 방해하지 않도록, 사망한 좀비의 콜라이더 컴포넌트를 가져와 모두 비활성화함.
        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
            // 남아있는 콜라이더 컴포넌트가 살아있는 좀비와 충돌하여 이동을 방해하지 않도록 비활성화
            zombieColliders[i].enabled = false;
        }

        // 죽은 좀비의 내비메시 에이전트 이동 중단
        navMeshAgent.isStopped = true;
        // 내비메시 에이전트 컴포넌트 비활성화 -> 내비메시 에이전트 컴포넌트가 살아있으면,
        // 다른 좀비 게임 오브젝트의 내비메시 에이전트가 이미 죽은 좀비의 내비메시 에이전트의 위치를 고려해서 경로를 계산하기 때문에
        // 죽은 좀비를 굳이 피해가는 경로로 이동하게 됨. -> 굳이 이렇게 죽은 좀비를 피해다니지 않도록 내비메시 에이전트 컴포넌트 자체를 비활성화함.
        navMeshAgent.enabled = false;

        // 사망 애니메이션 재생
        // Die 트리거 파라미터 실행으로 Any State -> Die 로 상태 전이 
        zombieAnimator.SetTrigger("Die");
        // 사망 효과음 재생
        zombieAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other) {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
    }
}