using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        while (i < 10)
        {
            Debug.Log(i + "번째 루프입니다.");
            i++;
        }
    }

}
