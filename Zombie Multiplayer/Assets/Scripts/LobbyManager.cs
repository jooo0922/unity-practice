using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks {
    private string gameVersion = "1"; // 게임 버전 -> 동일한 버전의 게임으로 접속한 플레이어들끼리만 매칭시키기 위해 사용하는 변수

    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start() {
        // 마스터 서버(매치메이킹 서버. 포톤의 전용 클라우드 서버 사용) 접속 옵션 설정 (게임버전만 설정)
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 옵션으로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 마스터 서버 접속 성공 전까지는 룸 접속을 못하도록 룸 접속 버튼 > 버튼 컴포넌트 > Interactable 비활성화 > 버튼 클릭 방지
        joinButton.interactable = false;
        // 마스터 서버 접속 시도 중임을 텍스트로 표시하도록 Connection Info Text 게임 오브젝트 > 텍스트 컴포넌트 > 텍스트 필드 내용 수정
        connectionInfoText.text = "마스터 서버에 접속 ...";
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster() {
        
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause) {
        
    }

    // 룸 접속 시도
    public void Connect() {
        
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message) {
        
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom() {
        
    }
}