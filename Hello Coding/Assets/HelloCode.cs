using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int love = 100;

        if (love > 70)
        {
            Debug.Log("굿엔딩: 히로인과 사귀게 되었다!");
        }
        if (love <= 70)
        {
            Debug.Log("배드엔딩: 히로인에게 차였다.");
        }
    }

}
