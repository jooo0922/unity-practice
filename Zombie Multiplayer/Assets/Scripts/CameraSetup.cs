using Cinemachine; // 시네머신 관련 코드
using Photon.Pun; // PUN 관련 코드
using UnityEngine;

// 시네머신 카메라가 로컬 플레이어를 추적하도록 설정
// MonoBehaviourPun 는 현재 게임 오브젝트에 추가된 Photon View 컴포넌트에 쉽게 접근할 수 있는 photonView 프로퍼티를 제공하는 클래스
public class CameraSetup : MonoBehaviourPun {
    void Start() {
        // 현재 게임 오브젝트가 로컬 게임 오브젝트인지 검사
        if (photonView.IsMine)
        {
            // 현재 씬에서 CinemachineVirtualCamera 컴포넌트를 갖고 있는 게임 오브젝트(Follow Cam)을 찾아옴
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            // 가상카메라 게임 오브젝트의 추적 대상 및 lookAt 을 현재 게임 오브젝트의 트랜스폼 컴포넌트 값으로 할당
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}