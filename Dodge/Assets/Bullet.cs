using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f; // 탄알 이동 속력
    private Rigidbody bulletRigidbody; // 이동에 사용할 리지드바디 컴포넌트

    // Start is called before the first frame update
    void Start()
    {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 bulletRigidbody 변수에 할당 (어떤 컴포넌트를 가져올 지 정의할 때, 제네릭 사용)
        bulletRigidbody = GetComponent<Rigidbody>();
        // 리지드바디의 속도 = 게임 오브젝트의 앞쪽 방향을 나타내는 방향벡터 * 이동 속력
        bulletRigidbody.velocity = transform.forward * speed;

        // 3초 뒤에 자신의 게임 오브젝트 파괴 (두 번째 인자값이 Destroy() 메서드 실행 지연시간)
        Destroy(gameObject, 3f);
    }
}
