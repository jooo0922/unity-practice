using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
   public AudioClip deathClip; // 사망시 재생할 오디오 클립
   public float jumpForce = 700f; // 점프 힘

   private int jumpCount = 0; // 누적 점프 횟수
   private bool isGrounded = false; // 바닥에 닿았는지 나타냄
   private bool isDead = false; // 사망 상태

   private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
   private Animator animator; // 사용할 애니메이터 컴포넌트
   private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트

   private void Start() {
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 멤버변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
   }

   private void Update() {
       // 사용자 입력을 감지하고 점프하는 처리
       if (isDead)
        {
            // 사망 시 현재의 Update() 루프 처리를 중단
            return;
        }

       // 마우스 왼쪽 버튼 누른 시점 && 현재까지의 누적 점프 횟수가 아직은 2보다 작은 시점 (최대 점프 횟수 = 2)
       if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            // 누적 점프 횟수 증가시킴
            jumpCount++;
            // 점프 직전 리지드바디 속도를 제로(0, 0)으로 초기화 -> 기존에 남아있는 다른 힘(속력)과 상쇄를 막기 위해 점프 직전 속도 초기화
            playerRigidbody.velocity = Vector2.zero; // Vector2.zero = (0, 0)
            // 리지드바디에 y축으로 가속도 주기 -> 점프 실행
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            // 오디오 소스 재생 -> 오디오 소스 컴포넌트의 Play() 메서드는 해당 컴포넌트에 할당된 오디오 클립을 재생함
            playerAudio.Play();
        } else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            // 마우스 왼쪽 버튼에서 손을 뗀 시점 && 현재 Player 게임 오브젝트 속도가 0보다 큰 시점 -> 즉, 점프 중인 시점 (0보다 작으면 낙하 중인 시점이겠지?)일 때,
            // 현재 속도를 절반으로 감속시킴 -> 오래 누를수록 점프 속도 감속이 지연되겠네 -> 오래 누를수록 점프 높이 증가
            // 또, 낙하 시점에는 감속이 되면 낙하가 느려지므로, 이거는 이상하잖아? 그러니까 낙하 시점에는 감속 로직을 실행하지 않으려는 것!
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        // 애니메이터 컴포넌트의 Grounded 파라미터를 계속 업데이트시킴
        // false 로 할당시키면 Grounded 가 false 인 전이조건 달성하여 Run -> Jump 로 상태 전이
        // true 로 할당시키면 Grounded 가 true 인 전이조건 달성하여 Jump -> Run 로 상태 전이
        animator.SetBool("Grounded", isGrounded);
   }

   private void Die() {
        // 사망 처리
        // 애니메이터 컴포넌트의 Die 트리거 파라미터 셋
        // SetTrigger() 메서드는 트리거 파라미터의 방아쇠를 당겨서, 파라미터의 값을 순간 true 로 만들었다가 다시 false 로 복귀시킴.
        // 이떄, Die 트리거 파라미터의 전이조건을 만족함에 따라, 현재 상태가 뭐가됬든, Any State -> Die 로 상태 전이
        animator.SetTrigger("Die");

        // 오디오 소스 컴포넌트에 할당된 오디오 클립을 deathClip 으로 변경 후, 변경된 오디오 클립 재생
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // 사망 시점에 Player 게임 오브젝트 속도값을 (0, 0) 으로 초기화시켜서 게임 오브젝트를 멈춰세움
        playerRigidbody.velocity = Vector2.zero;

        // 사망상태를 나타내는 멤버변수를 true 로 변경
        isDead = true;
   }

   private void OnTriggerEnter2D(Collider2D other) {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
   }

   private void OnCollisionEnter2D(Collision2D collision) {
       // 바닥에 닿았음을 감지하는 처리
   }

   private void OnCollisionExit2D(Collision2D collision) {
       // 바닥에서 벗어났음을 감지하는 처리
   }
}