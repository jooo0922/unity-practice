using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // 생성알 탄알 게임 오브젝트의 원본 프리팹
    public float spawnRateMin = 0.5f; // 최소 생성 주기
    public float spawnRateMax = 3f; // 최대 생성 주기

    private Transform target; // 발사할 대상
    private float spawnRate; // 랜덤한 탄알 생성 주기
    private float timeAfterSpawn; // 마지막 탄알 생성 시점 이후 흐른 시간 -> 일종의 '타이머'

    // Start is called before the first frame update
    void Start()
    {
        // 마지막 탄알 생성 시점 이후의 누적시간 값을 0으로 초기화
        timeAfterSpawn = 0f;

        // 탄알 생성 간격을 'spawnRateMin ~ spawnRateMax' 사이의 랜덤값으로 지정 (범위값을 실수로 받으면 리턴값도 실수를 전달함)
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);

        // PlayerController 컴포넌트를 갖고있는 게임 오브젝트를 찾고, 해당 오브젝트의 Transform 컴포넌트 참조값을 멤버변수에 할당함. -> 탄알이 향할 목표 게임 오브젝트의 위치를 알기 위함.
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }                                                                                                                                                                          
}
