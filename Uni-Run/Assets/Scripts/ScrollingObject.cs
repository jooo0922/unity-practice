using UnityEngine;

// 게임 오브젝트를 계속 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour {
    public float speed = 10f; // 이동 속도

    private void Update() {
        // 게임 오브젝트를 왼쪽으로 일정 속도로 평행 이동하는 처리
        // 초당 Vector3(-speed, 0, 0) 만큼 왼쪽으로 평행이동
        // 참고로, Vector3.left = Vector3(-1, 0, 0) 방향벡터의 속기 표현이고,
        // Translate() 메서드는 게임 오브젝트의 오브젝트 공간(또는 설정에 따라 전역공간) 기준으로 인자로 전달한 Vector3 만큼 평행이동시킴!
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}