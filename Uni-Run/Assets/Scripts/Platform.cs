using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물 오브젝트들
    private bool stepped = false; // 플레이어 캐릭터가 밟았었는가

    // 컴포넌트가 활성화될때 마다 매번 실행되는 메서드
    // 인스펙터 창에서 컴포넌트 옆의 체크버튼을 활성화/비활성화하거나, 해당 컴포넌트가 포함된 게임 오브젝트 옆의 체크버튼을 활성화/비활성화해서
    // 모든 컴포넌트가 일률적으로 활성화/비활성화될 때 실행되는 유니티 이벤트 메서드
    private void OnEnable() {
        // 발판을 리셋하는 처리
        // 밟힘 상태를 리셋함
        stepped = false;

        // 인스펙터 창에서 할당된 장애물 게임 오브젝트 리스트 배열을 반복 순회
        for (int i = 0; i < obstacles.Length; i++)
        {
            // Random.Range(0, 3) 은 0에서 3 사이의 정수 0, 1, 2 중 하나를 랜덤으로 반환함
            if (Random.Range(0, 3) == 0)
            {
                // 현재 순회를 돌고 있는 장애물 게임 오브젝트를 1/3 확률로 활성화함
                obstacles[i].SetActive(true);
            } else
            {
                // 반대로 현재 장애물 게임 오브젝트를 2/3 확률로 비활성화함
                obstacles[i].SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // 플레이어 캐릭터가 자신을 밟았을때 점수를 추가하는 처리
        // 충돌한 상대방 게임 오브젝트의 태그가 Player 이고, (Player 게임 오브젝트의 인스펙터 창에서 설정해놨음!)
        // 발판 중복 밟힘에 의한 점수 중복 추가 방지를 위해 stepped 가 아직 밟히지 않은 상태인지 체크
        if (collision.collider.tag == "Player" && !stepped)
        {
            stepped = true; // 밟힘 상태를 true 로 체크해서 다음번 중복 밟힘에 의한 점수 중복 추가 버그를 방지
            GameManager.instance.AddScore(1); // GameManager 오브젝트의 싱글턴 인스턴스 정적 멤버변수에 접근해서, 점수를 추가해서 점수 UI 게임 오브젝트를 갱신하는 메서드 실행
        }
    }
}