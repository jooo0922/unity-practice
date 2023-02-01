using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform childTransform; // 움직일 자식 게임 오브젝트의 Transform 컴포넌트 -> 인스펙터 창에서 참조값을 할당하려고 public 설정함

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -1, 0); // 게임오브젝트 자신의 전역위치를 초기화함.
        childTransform.localPosition = new Vector3(0, 2, 0); // 게임오브젝트의 자식의 지역위치 (부모 게임오브젝트 기준 상대적 위치) 를 초기화함.

        // Transform.rotation 멤버변수는 내부적으로 쿼터니온 타입으로 처리하고 있으므로, 코드로 오일러 각을 설정하려면
        // Vector3 오일러각을 쿼터니온 타입으로 변환하는 메서드 Quaternion.Euler() 를 사용하여 쿼터니온 타입으로 변환해서 업데이트해줘야 함.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30)); // 자신의 전역각도를 초기화함.
        childTransform.localRotation = Quaternion.Euler(new Vector3(0, 60, 0)); // 자식의 지역각도 (부모 게임오브젝트 기준 상대적 각도) 를 초기화함.
    }

    // Update is called once per frame
    void Update()
    {
        // 위쪽 방향키 입력 시,
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // 게임오브젝트 자신의 오브젝트 공간 좌표 기준 (게임 오브젝트 자신의 방향 기준) 초당 (0, 1, 0) 만큼 평행이동
            // Translate() 메서드는 기본적으로 오브젝트 공간 기준으로 평행이동 시키고, 이를 전역공간 기준으로 바꾸려면 두 번째 인자에 Space.World 를 넣어주면 됨. 
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime);
        }

        // 아래쪽 방향키 입력 시,
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // 게임오브젝트 자신의 오브젝트 공간 좌표 기준 초당 (0, -1, 0) 만큼 평행이동
            // Time.deltaTime 값은 60FPS 기준 1/60 인데, 60FPS 가 지날 동안 (=1초 동안) Vector3(0, -1, 0) * 1/60 이동값이 누적되면
            // 1초 동안 사실상 (0, -1, 0) 만큼 이동한 게 되니까, 초당 (0, -1, 0) 만큼 평행이동 하는게 맞음!
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime);
        }

        // 왼쪽 방향키 입력 시,
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // 게임 오브젝트 자신의 오브젝트 공간 좌표 기준 초당 (0, 0, 180) 만큼 회전
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);
            // 게임 오브젝트의 자식의 오브젝트 공간 좌표 기준 초당 (0, 180, 0) 만큼 회전
            childTransform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
        }

        // 오른쪽 방향키 입력 시,
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // 게임 오브젝트 자신의 오브젝트 공간 좌표 기준 초당 (0, 0, -180) 만큼 회전
            transform.Rotate(new Vector3(0, 0, -180) * Time.deltaTime);
            // 게임 오브젝트의 자식의 오브젝트 공간 좌표 기준 초당 (0, -180, 0) 만큼 회전
            childTransform.Rotate(new Vector3(0, -180, 0) * Time.deltaTime);
        }
    }
}

// 유니티의 '지역공간' 이 갖는 두 가지 의미
//
// 1. 게임 오브젝트의 위치, 각도, 스케일값 변경 시, (인스펙터 창 또는 코드의 Transform.position / .rotation / .scale)
// 게임 오브젝트의 '지역공간'(부모 오브젝트의 상대적 좌표) 기준으로 변경되고,
//
// 2. 게임 오브젝트의 평행이동, 회전 적용 시, (Local 모드에서 평행이동 툴, 회전 툴 사용 또는 Transform.Translate() / .Rotate())
// 게임 오브젝트의 '오브젝트 공간'(게임 오브젝트 자신의 상대적 좌표) 기준으로 변경됨.
