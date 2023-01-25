using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody; // 리지드바디 컴포넌트 private 으로 지정 -> 더 이상 인스펙터 창에서 드래&드롭으로 가져오지 않을거기 때문!
    public float speed = 8f; // 이동 속력

    void Start()
    {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 playerRigidbody 멤버변수에 코드로 직접 할당!
        // 제네릭을 사용해서 컴포넌트 타입마다 별도의 게터 메서드를 생성하지 않아도 하나의 GetComponent() 로 대응할 수 있도록 함!
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 수평축 (왼쪽 / 오른쪽 방향키 또는 A / D)과 수직축 (위쪽 / 아래쪽 방향키 또는 W / S)의 입력값을 감지하여 저장
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // 실제 이동 속도를 입력값과 이동 속력을 사용해서 계산
        // xInput 과 zInput 값은 일종의 방향을 결정하는 방향값 역할!
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        // Vector3 속도를 (xSpeed, 0, zSpeed)로 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);

        // 리지드바디의 속도에 newVelocity 할당
        // AddForce() 와의 차이점 -> AddForce() 는 가속도가 붙는 개념이라 관성이 적용되고 조작이 무겁게 느껴짐
        // 반면, Rigidbody.velocity 는 이전 속도를 지우고 새로운 속도를 override 하는 개념이라 속도가 즉시 변경되는 차이가 있음!
        playerRigidbody.velocity = newVelocity;
    }

    public void Die()
    {
        // 자신의 게임 오브젝트를 비활성화 (게임 오브젝트의 Inspector 창에서 체크박스 해제하는 것과 동일한 역할)
        gameObject.SetActive(false);

        // 씬에 존재하는 GameManager 타입의 오브젝트를 찾아 가져옴.
        GameManager gameManager = FindObjectOfType<GameManager>();
        // GameManager 오브젝트의 EndGame() 메서드 실행 -> PlayerController.Die() 실행 시, 자동으로 GameManager.EndGame() 도 실행되겠군!
        gameManager.EndGame();
    }
}
