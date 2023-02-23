using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour {
    public Gun gun; // 사용할 총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    private void Start() {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        // 슈터가 활성화될 때 총도 함께 활성화
        gun.gameObject.SetActive(true);
    }
    
    private void OnDisable() {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        gun.gameObject.SetActive(false);
    }

    private void Update() {
        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.fire)
        {
            // 발사 입력 감지 시, Gun 스크립트의 총 발사 시도 메서드 실행
            gun.Fire();
        }
        else if (playerInput.reload)
        {
            // 재장전 입력 감지 시, Gun 스크립트의 재장전 시도 메서드 실행
            // 재장전 시도 메서드 실행결과(true / false 반환. Gun 스크립트 참고) 검사
            if (gun.Reload())
            {
                // 재장전 성공 시, 애니메이터 컨트롤러의 Reload 트리거 파라미터 발동 > Upper Body 레이어 상태도의 Reload 애니메이션 실행
                // Reload -> Aim Idle(기본 상태) 로의 전이조건은 없기 때문에, Reload 애니메이션이 끝나면 알아서 기본 상태로 돌아옴.
                playerAnimator.SetTrigger("Reload");
            }
        }

        // 남은 탄알 UI 갱신
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI() {
        if (gun != null && UIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex) {
        
    }
}