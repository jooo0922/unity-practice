using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
// MonoBehaviourPunCallbacks 클래스는 MonoBehaviour 클래스를 상속받고 있으며,
// 기존 유니티 이벤트 메서드에 더해서 포톤 서비스에 의해 발생할 수 있는 이벤트(마스터서버 접속 성공, 마스터서버 접속 실패, 룸 접속 성공, 룸 접속 실패 등...)를 감지하고, 해당 콜백 이벤트 메서드를 자동실행 해줌!
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
    // 부모 클래스인 MonoBehaviourPunCallbacks 이 마스터 서버 접속 성공 이벤트 OnConnectedToMaster 를 감지해서
    // 호출시킬 이벤트 콜백 메서드를 정의하려는 것!
    public override void OnConnectedToMaster() {
        // 마스터 서버 접속이 성공했다, 룸 접속이 가능하도록 버튼 활성화
        joinButton.interactable = true;
        // 마스터 서버 접속 성공 했음을 텍스트로 표시
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause) {
        // 룸 접속 버튼 > 버튼 컴포넌트 > Interactable 속성 비활성화 유지
        joinButton.interactable = false;
        // 마스터 서버 접속 실패 했음을 텍스트로 표시
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";

        // Start() 에서 호출한 것과 동일한 마스터 서버 접속 메서드 호출
        PhotonNetwork.ConnectUsingSettings();
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