using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity {
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    // Awake() 는 Start() 와 유사하게 초기 1회 자동 실행되는 유니티 이벤트 메서드
    // 다만, Start() 보다 실행시점이 한 프레임 더 빠름
    private void Awake() {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    // LivingEntity 의 OnEnable() 메서드 확장
    protected override void OnEnable() {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        // 플레이어 사망 시, Die() 메서드에서 비활성화 해주는 게임 오브젝트와 컴포넌트들을 다시 활성화하는 것.
        // 이는 플레이어 캐릭터의 '부활 기능'을 염두에 둔 기능이라고 보면 됨. -> 사망 후 바로 게임오버 처리할거면 굳이 런타임에 얘내들을 다시 활성화해주는 작업은 안해줘도 됨.
        // 체력 슬라이더 활성화
        healthSlider.gameObject.SetActive(true);
        // 체력 슬라이더 컴포넌트의 최댓값 필드를 기본 체력값으로 초기화
        healthSlider.maxValue = startingHealth;
        // 체력 슬라이더 컴포넌트의 값 필드를 현재 체력값으로 초기화
        healthSlider.value = health;

        // 플레이어 조작 관련 스크립트 컴포넌트 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        // 체력 슬라이더 컴포넌트의 값 필드를 회복된 현재 체력으로 업데이트
        healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection) {
        if (!dead)
        {
            // 플레이어 캐릭터가 사망하지 않, 맞기만 했을 때 오디오 클립 재생 (PlayOneShot() -> 이전 오디오와 중첩 재생)
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        // 체력 슬라이더 컴포넌트의 값 필드를 데미지가 적용되어 깎인 현재 체력으로 업데이트
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        // 체력 슬라이더 게임 오브젝트 비활성화 -> 사망했으니 체력이 바닥났을테고, 그럼 안보여줘도 되겠지.
        healthSlider.gameObject.SetActive(false);

        // 사망 오디오 클립 재생
        playerAudioPlayer.PlayOneShot(deathClip);
        // 애니메이터 컴포넌트의 Die 트리거 파라미터 발동(일시로 Die 파라미터가 true 가 됬다가 다시 false 로 복구) > Die 상태로 일시 전이 > 사망 애니메이션 재생
        playerAnimator.SetTrigger("Die");

        // 플레이어 조작 관련 스크립트 컴포넌트 비활성화
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        // 플레이어 캐릭터가 사망하지 않은 경우에만 아이템을 사용할 수 있도록 함
        if (!dead)
        {
            // 충돌한 상대방 콜라이더 컴포넌트 > 상대방 게임 오브젝트 > IItem 인터페이스를 상속받는 컴포넌트를 갖고 있는지 검색
            IItem item = other.GetComponent<IItem>();

            // 충돌한 상대방 게임 오브젝트가 IItem 을 상속받는 아이템 게임 오브젝트라면 해당 블록을 실행함
            if (item != null)
            {
                // Use() 메서드를 실행하여 해당 아이템 사용
                // 아이템 사용 대상은 플레이어 캐릭터 게임 오브젝트 자신을 인자로 전달해 줌.
                item.Use(gameObject);
                // 아이템 줍는 오디오 클립 재생
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
}