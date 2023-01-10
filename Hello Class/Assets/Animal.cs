using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 순수한 클래스의 역할만 살펴보기 위해 Animal 클래스는 MonoBehaviour 클래스를 상속받지 않음 -> 컴포넌트로써의 기능 상실!
public class Animal
{
    // 동물에 대한 변수
    public string name;
    public string sound;

    // 울음소리를 재생하는 메서드
    public void PlaySound()
    {
        Debug.Log(name + " : " + sound);
    }
        
}
