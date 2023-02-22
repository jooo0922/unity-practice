using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake() {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>(); // 오디오소스 컴포넌트 가져오기
        bulletLineRenderer = GetComponent<LineRenderer>(); // 라인 렌더러 컴포넌트 가져오기

        // 라인 렌더러가 사용할 점 2개로 변경 (총구 위치, 총알이 닿는 위치)
        bulletLineRenderer.positionCount = 2;

        // 라인 렌더러 비활성화 (이미 인스펙터 창에서 비활성화 했지만, 코드에서도 확실하게 비활성화하는 게 좋음)
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable() {
        // 총 상태 초기화
        // 전체 예비 탄알 초기화
        ammoRemain = gunData.startAmmoRemain;
        // 현재 탄창 속 탄알 초기화(가득 채움)
        magAmmo = gunData.magCapacity;

        // 총의 현재 상태를 준비 상태로 변경
        // 'state = ' 하는 순간 프로퍼티 set 접근자로 상태값을 변경한 것임!
        state = State.Ready;
        // 마지막으로 총을 쏜 시점 초기화
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire() {
        // 총 발사 조건 검사
        // 1. 현재 총 상태가 State.Ready 인지
        // 2. 현재 시점 >= 마지막 총 발사 시점 + 탄알 발사 간격 보다 지났는지
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            // 두 가지 조건을 모두 만족하면 아래 작업을 처리
            // 총 발사 시점을 현재 시점으로 갱신
            lastFireTime = Time.time;
            // 실제 발사 메서드 호출 (Fire() 는 조건을 체크해서 실제 발사 작업을 '위임'하는 역할)
            Shot();
        }

    }

    // 실제 발사 처리
    private void Shot() {
        // 레이캐스트에 의한 충돌 정보를 저장하는 RaycastHit 타입의 컨테이너 변수 선언 (충돌 위치, 충돌 대상, 충돌 표면의 방향 등의 정보를 담고 있음.)
        RaycastHit hit;
        // 탄알이 맞은 곳 (레이캐스트와의 충돌 위치)를 저장할 변수. (Vector3(0, 0, 0) 으로 초기화)
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트 충돌 여부 검사 (레이(반직선) 시작점, 레이 방향벡터, 충돌 정보 저장 컨테이너 변수, 레이 길이(탄알의 사정거리))
        // 참고로 RaycastHit 타입 변수 hit 앞에 out 을 붙인 것, Raycast() 메서드 내에서 해당 변수가 변경되면, 변경사항을 유지한 채 반환해주는 키워드임.
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            // 레이캐스트 충돌 검사 결과, 어떤 게임 오브젝트와 충돌한 경우,

            // 충돌한 상대방 게임 오브젝트의 콜라이더 컴포넌트를 통해, IDamageable 인터페이스를 상속받는 컴포넌트를 찾음
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            // IDamageable 인터페이스를 상속받은 컴포넌트를 갖고 있다는 것은, 충돌한 상대방 게임 오브젝트가 데미지를 입을 수 있는 게임 오브젝트라는 뜻!
            if (target != null)
            {
                // 상대방 게임 오브젝트의 IDamageable 인터페이스를 상속받는 컴포넌트의 OnDamage() 메서드를 실행해서 상대방에게 데미지를 입힘
                // RaycastHit.point 는 레이캐스트의 충돌위치, RaycastHit.normal 은 레이캐스트의 충돌 표면 방향벡터 값을 갖는 필드
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이캐스트 충돌 검사 결과, 충돌한 게임 오브젝트가 없는 경우,

            // 충돌위치값을 총구위치로부터 총구 앞쪽방향으로 최대 사정거리까지 더해준 위치값(즉, 총알이 최대 사정거리까지 날아갔을 때의 위치)을 저장함/
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        // 발사 이펙트를 재생하는 코루틴 메서드 실행
        // StartCoroutine() 메서드는 코루틴 메서드를 실행시키는 유니티 내장 메서드
        StartCoroutine(ShotEffect(hitPosition));

        // 현재 탄알집에 남은 탄알 수 -1
        magAmmo--;
        if (magAmmo <= 0)
        {
            // 현재 탄알집에 탄알이 없다면, 총의 현재 상태를 Empty 로 변경
            state = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition) {
        // 총구 화염 파티클 시스템 컴포넌트 재생
        muzzleFlashEffect.Play();
        // 탄피 배출 파티클 시스템 컴포넌트 재생
        shellEjectEffect.Play();

        // 총격 소리 오디오 클립 재생
        // 오디오 소스 컴포넌트의 PlayOneShot() 메서드는 이미 재생중인 오디오 클립을 정지하지 않고, 입력받은 오디오 클립과 같이 재생시켜 소리를 중첩시킴! -> 총알 연사 효과음에 적합!
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // 라인 렌더러로 그릴 선(탄알 궤적)의 시작점과 끝점 정의
        bulletLineRenderer.SetPosition(0, fireTransform.position); // 시작점: 총구 위치
        bulletLineRenderer.SetPosition(1, hitPosition); // 끝점: 탄알이 맞은 위치 (코루틴 메서드 ShotEffect() 의 인자로 전달받음)

        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {
        // 1. 현재 재장전 중이거나
        // 2. 남은 탄알이 없거나
        // 3. 현재 탄알집에 탄알이 최대치인 경우
        // 재장전을 실행하지 않고 false 를 반환하며 Reload() 메서드를 종료함
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        // 위 조건 중 하나라도 해당되면 재장전을 실행시킴
        // 실질적인 재장전 작업은 코루틴 메서드 ReloadRoutine() 에서 실행 -> Reload() 는 재장전 작업을 위임하는 역할
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환 -> 재장전 및 발사 불가 -> 사실상 총을 잠금상태로 만든 것! 
        state = State.Reloading;
        // 재장전 오디오 클립 재생 -> 이전 오디오 클립 소리와 중첩시켜 재생
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
      
        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 탄창에 채워야 할 탄알 수 계산 (탄알집 최대 탄알 수 - 탄알집 현재 탄알 수)
        int ammoToFill = gunData.magCapacity - magAmmo;

        // 탄창에 채워야 할 탄알 수가 남아있는 전체 탄알 수보다 많다면,
        // 남아있는 탄알 수 만큼이라도 채우도록 채워야 할 탄알 수 변경
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        // 현재 탄알집을 채움
        magAmmo += ammoToFill;
        // 남아있는 전체 탄알 수에서 채운 탄알 수만큼 빼줌
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경 -> 발사 가능. 탄알집 탄알 수 감소 시, 재장전도 가능. -> 사실상 총의 잠금을 해제한 것!
        state = State.Ready;
    }
}