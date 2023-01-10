using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animal tom = new Animal();
        tom.name = "톰";
        tom.sound = "냐옹!";

        Animal jerry = new Animal();
        jerry.name = "제리";
        jerry.sound = "찍찍!";

        jerry = tom; // jerry 가 가리키는 참조값을 tom 이 가리키는 참조값으로 변경 
        jerry.name = "미키"; // jerry 가 가리키는 인스턴스가 바뀐 상태에서, 그 인스턴스의 name 값을 바꾸고 있음!

        tom.PlaySound();
        jerry.PlaySound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
