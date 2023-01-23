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
        
    }

    // 현재 게임을 게임오버 상태로 변경하는 메서드 (상태값 변경 메서드) -> 일단 메서드 선언만 해둠
    public void EndGame()
    {

    }
}
