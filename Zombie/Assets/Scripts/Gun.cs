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
        return false;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
      
        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}