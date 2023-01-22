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
        // timeAfterSpawn 시간값 누적
        // Time.deltaTime -> 직전 프레임이 그려진 시점에서 현재 프레임이 그려진 시간까지의 시간 간격
        timeAfterSpawn += Time.deltaTime;

        // 최근 탄알 생성 시점에서부터 누적된 시간값이 랜덤하게 할당된 탄알 생성 주기값을 넘어서면
        if (timeAfterSpawn >= spawnRate)
        {
            // 누적된 시간값 초기화
            timeAfterSpawn = 0f;

            // bulletPrefab 원본을 복제한 탄알 게임 오브젝트 생성
            // 또한 복제된 탄알의 위치 및 회전값을 BulletSpawner(탄알 생성기) 자신의 Transform 컴포넌트와 맞춤 -> why? 탄알이 탄알생성기에서 튀어나오니까!
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

            // 복제하여 생성된 탄알 게임 오브젝트의 정면 방향이 target (즉, Player 게임 오브젝트) 를 바라보도록 Transform.LookAt() 을 사용하여 회전시킴
            // LookAt 의 인자에는 바라보고자 하는 게임 오브젝트의 Transform 컴포넌트를 전닳해야 함. -> Start() 메서드에서 target 변수에 Player 게임 오브젝트의 Transform 컴포넌트를 할당했었지?
            bullet.transform.LookAt(target);

            // 다음번 탄알 생성 주기를 또 다른 랜덤값으로 변경해 줌.
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }                                                                                                                                                                          
}
