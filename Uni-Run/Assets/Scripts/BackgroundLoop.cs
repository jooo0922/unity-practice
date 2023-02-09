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
        // 현재 위치에서 오른쪽으로 width * 2 만큼 순간이동하는 로직
        Vector2 offset = new Vector2(width * 2f, 0); // 현재 위치에서 얼마나 오른쪽으로 이동할건지 거리를 정의한 Vector2 변수

        // 여기서 서로 다른 두 타입의 변수 Vector2 와 Vector3 를 가지고 연산을 하고 있는데,
        // 1. 첫 번째 연산인 Vector3 + Vector2 덧셈 연산은 Vector3 인 transform.position 을 반드시 Vector2 로 형변환해준 뒤 연산해줘야 함.
        // 2. 두 번째 연산인 Vector3 변수 = Vector2 결과값 할당은 transform.position 을 명시적으로 형변환해줄 필요는 없음.
        // 결론을 요약하면, 덧셈 연산은 형변환 필수, 할당은 형변환 필수는 아니다!
        transform.position = (Vector2)transform.position + offset;
    }
}