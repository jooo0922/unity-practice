using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour {
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public Text scoreText; // 점수를 출력할 UI 게임 오브젝트의 텍스트 컴포넌트 (public 이니까 인스펙터 창에서 참조값 할당하겠지?)
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트 (public 이니까 인스펙터 창에서 참조값 할당하겠지?)

    private int score = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake() {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Update() {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        // 현재 게임오버 상태이고, 왼쪽 마우스 버튼을 클릭한 시점이라면, if block 실행!
        if (isGameover && Input.GetMouseButtonDown(0))
        {
            // SceneManager.LoadScene("씬 이름"); 은 빌드 목록에 등록된 씬의 이름을 받아, 현재까지 남아있는 해당 씬을 파괴한 뒤, 다시 로드함.
            // SceneManager.GetActiveScene() 은 현재 활성화된 씬의 정보를 Scene 타입의 오브젝트로 반환함.
            // SceneManager.GetActiveScene().name 변수애는 현재 활성화된 씬의 이름이 담겨있음
            // 정리하면, 현재 활성화되어 있는 씬을 파괴했다가, 다시 로드해서 복구시키는 것 -> 게임 재시작!
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore) {
        // 게임오버 상태가 아닐 때에만 점수를 갱신
        if (!isGameover)
        {
            score += newScore; // 게임점수 멤버변수에 인자로 전달받은 추가점수를 더함
            scoreText.text = "Score : " + score; // 점수 출력 UI 게임 오브젝트 > 텍스트 컴포넌트 > 텍스트 필드(멤버변수) 에 갱신된 점수로 출력 텍스트 생성
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead() {
        isGameover = true; // 게임오버 상태를 true 로 변경
        gameoverUI.SetActive(true); // 게임오버 UI 게임오브젝트를 활성화 (인스펙터 창에서 비활성화 해뒀었지?)
    }
}