using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 유니티 UI 시스템 관련 라이브러리 가져오기
using UnityEngine.SceneManagement; // 씬 관리 관련 라이브러리 -> 게임 도중 씬 재시작 기능 구현 목적

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText; // 게임오버 시 활성화/비활성화 토글 처리를 위한 텍스트 게임 오브젝트
    public Text timeText; // 생존시간을 표시할 게임 오브젝트의 텍스트 필드 수정을 위한 텍스트 컴포넌트
    public Text recordText; // 최고기록을 표시할 게임 오브젝트의 텍스트 필드 수정을 위한 텍스트 컴포넌트

    private float surviveTime; // 게임 시작 후부터 플레이어가 생존해 있는 시간
    private bool isGameover; // 게임오버 상태

    // Start is called before the first frame update
    void Start()
    {
        // 각 상태값 초기화
        surviveTime = 0;
        isGameover = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 생존해있어 게임오버가 아닌 상태에서만 아래 갱신 로직 실행
        if (!isGameover)
        {
            // 생존 시간을 델타타임(프레임 간 시간간격)을 누적하여 업데이트함.
            surviveTime += Time.deltaTime;

            // 갱신된 생존시간을 float -> int 로 형변환 후, Time Text 게임 오브젝트 > 텍스트 컴포넌트 > 텍스트 필드에 할당해 줌.
            // 형변환을 해준 이유는, float 실수 타입은 소수점이 길이서, 이 긴 숫자들이 전부 화면에 표시되지 않도록 정수형으로 끊어서 표시하려는 것임.
            timeText.text = "Time: " + (int)surviveTime;
        }
        else
        {
            // 게임오버 상태에서 R키를 누른 순간 아래의 if block 실행
            if (Input.GetKeyDown(KeyCode.R))
            {
                // 현재 활성화된 씬을 제거해버리고, SampleScene 이라는 이름으로 빌드목록에 등록된 씬을 로드함.
                // 참고로 SceneManager.LoadScene() 은 UnityEngine.SceneManagement 모듈에서 가져올 수 있는 메서드임.
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    // 현재 게임을 게임오버 상태로 변경하는 메서드 (상태값 변경 메서드) -> 일단 메서드 선언만 해둠
    public void EndGame()
    {

    }
}
