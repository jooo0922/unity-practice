using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackgroundLoop : MonoBehaviour {
    private float width; // 배경의 가로 길이

    // Awake() 는 Start() 와 유사하게 초기 1회 자동 실행되는 유니티 이벤트 메서드
    // 다만, Start() 보다 실행시점이 한 프레임 더 빠름
    private void Awake() {
        // 가로 길이를 측정하는 처리
        // BoxCollider2D 컴포넌트를 가져와서, 사이즈 값을 참조해서 sky 게임 오브젝트의 width 를 구함.
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x; // 참고로 BoxCollider2D 의 사이즈값은 Sky 게임 오브젝트의 인스펙터 창에서도 확인 가능!
    }

    private void Update() {
        // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 리셋
        // 매 프레임마다 -width 보다 x축 좌표값이 더 왼쪽으로 이동한 상태인지 검사함
        if (transform.position.x <= -width)
        {
            Reposition(); // 게임 오브젝트의 position (부모 오브젝트 기준의 지역공간 좌표)를 width 로 순간이동 시키는 메서드 실행
        }
    }

    // 위치를 리셋하는 메서드
    private void Reposition() {
        
    }
}