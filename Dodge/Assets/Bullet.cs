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

    // 트리거 충돌 시 자동으로 실행되는 이벤트 메서드 -> 실제 물리적인 충돌이 아니여도, 최소 한 개의 게임 오브젝트가 트리거 콜라이더라서 '충돌 감지'만 일어나면 발생!
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 상대방 게임 오브젝트가 Player 태그를 가진 경우 -> 즉, 상대방이 Player 게임 오브젝트인지 확인하려는 것! (tag 변수는 각 게임 오브젝트의 컴포넌트에도 존재!)
        if (other.tag == "Player")
        {
            // 상대방 게임 오브젝트에서 PlayerController 컴포넌트 가져오기
            // c# script 컴포넌트를 GetComponent() 로 찾고자 한다면, 제네릭 타입을 해당 스크립트에서 MonoBehavior 클래스로부터 상속받고 있는 클래스 이름으로 넣어주면 되겠지!
            PlayerController playerController = other.GetComponent<PlayerController>();

            // 상대방으로부터 PlayerController 컴포넌트를 가져오는 데 성공했다면(즉, 상대방 게임 오브젝트에 PlayerController 라는 컴포넌트가 존재한다면~)
            if (playerController != null)
            {
                // 상대방 PlayerController 컴포넌트의 Die() 메서드 실행
                playerController.Die();
            }
        }
    }
}

