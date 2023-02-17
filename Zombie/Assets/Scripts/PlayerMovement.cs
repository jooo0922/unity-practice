using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도


    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    // 유니티 내부의 물리 엔진 실행주기가 0.2로 고정되어 있어, 이 주기에 맞춰 실행되는 업데이트 루프임.
    // 게임오브젝트의 이동, 충돌 등 물리적인 변화 처리 시, 물리 엔진의 예측을 방해하지 않도록 FixedUpdate() 사용이 권장됨.
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        // 회전 실행
        Rotate();

        // 이동 실행
        Move();

        // 입력값에 따라 애니메이터 컴포넌트 > 애니메이터 컨트롤러 > Move 파라미터 값 변경
        // 이 값을 애니메이터 컨트롤러 > Movement 상태의 블렌드 트리 > 각 애니메이션 클립별 Threshold 값과 비교해서
        // 기본 애니메이션 상태인 Movement 내의 클립들을 적절한 비율로 섞어서 재생시켜 줌.
        playerAnimator.SetFloat("Move", playerInput.move); // PlayerInput 컴포넌트의 move 프로퍼티 get 접근자 사용해서 값을 가져옴.
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        // 상대적으로 이동할 거리 계산
        // 정방향 / 반대방향 / 정지 여부 * 플레이어의 오브젝트공간 좌표계 기준 앞쪽방향 * 이동 속력 * 한 프레임 기준 이동거리로 변환하는 델타타임값
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;

        // 리지드바디 컴포넌트의 MovePosition() 메서드로 플레이어 게임 오브젝트의 전역위치 변경 (물리처리 적용하며 위치 변경)
        // 리지드바.MovePosition(이동하고자 하는 전역위치 Vector3 값) -> MovePosition(현재 위치 + 상대적으로 더 이동할 거리)
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        // 상대적으로 회전할 각도 계산
        // 정방향회전 / 반대방향회전 / 정지 여부 * 회전속력 * 한 프레임 기준 회전 각도로 변환하는 델타타임값
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // 리지드바디.rotation 에 회전하고자 하는 목표각도를 계산하여 할당
        // 현재 각도 * 상대적으로 더 회하고자 하는 각도 -> 쿼터니온에서 상대적으로 더 회전하려는 각도를 계산하려면 쿼터니온 곱으로 계산해줘야 함. -> 쿼터니온 사이의 연산이 행렬 곱이기 때문
        // 이떄, Vector3(0, turn, 0)은 오일러각 이므로, 유니티 내부에서 처리하는 쿼터니온 타입으로 변환하여 곱해줌.
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }
}