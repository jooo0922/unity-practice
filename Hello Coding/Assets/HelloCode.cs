using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // int 타입 원소들이 묶인 배열 타입임을 명시
        // js 와 달리, 배열 타입 생성 시점에 size 가 미리 지정되고, 배열을 마치 객체 인스턴스처럼 생성해서 사용함.
        int[] students = new int[5];

        students[0] = 100;
        students[1] = 90;
        students[2] = 80;
        students[3] = 70;
        students[4] = 60;

        Debug.Log("0 번 학생의 점수 : " + students[0]);
        Debug.Log("1 번 학생의 점수 : " + students[1]);
        Debug.Log("2 번 학생의 점수 : " + students[2]);
        Debug.Log("3 번 학생의 점수 : " + students[3]);
        Debug.Log("4 번 학생의 점수 : " + students[4]);
    }

}
