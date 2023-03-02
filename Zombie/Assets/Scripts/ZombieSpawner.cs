using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviour {
    public Zombie zombiePrefab; // 생성할 좀비 원본 프리팹

    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들

    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트
    private int wave; // 현재 웨이브

    private void Update() {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if (zombies.Count <= 0)
        {
            SpawnWave();
        }

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        // 현재 웨이브와 남은 적 수 표시
        UIManager.instance.UpdateWaveText(wave, zombies.Count);
    }

    // 현재 웨이브에 맞춰 좀비들을 생성
    private void SpawnWave() {
        // 현재 웨이브를 1 증가시킴
        wave++;

        // 현재 웨이브에 * 1.5 후, 반올림한 정수값만큼 좀비 생성
        // Mathf.RoundToInt(float) 은 float 타입을 입력받아 반올림 후, 정수로 반환하는 메서드 
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // spawnCount 수만큼 좀비 생성 -> 현재 웨이브 수에 비례하여 생성되는 좀비 수 증가
        for (int i = 0; i < spawnCount; i++)
        {
            // 실제 좀비 생성 메서드 실행
            CreateZombie();
        }
    }

    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    private void CreateZombie() {
        // 사용할 좀비 데이터 스크립터블 오브젝트 에셋을 랜덤하게 가져옴
        // 배열에 접근해서 가져올 인덱스를 0 ~ 좀비데이터 배열 개수 사이의 랜덤 정수로 반환받아 사용함
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];

        // 생성된 좀비 게임 오브젝트의 트랜스폼을 랜덤으로 가져옴
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate(좀비 원본 프리팹(에 속한 컴포넌트), 생성될 좀비 위치, 생성될 좀비 회전) 메서드로 좀비 게임 오브젝트의 인스턴스 생성
        // 참고로, Instantiate() 에 컴포넌트를 전달하면, 해당 컴포넌트가 포함된 원본 프리팹으로부터 새로운 게임 오브젝트를 생성해 줌.
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

        // 생성한 좀비의 능력치 설정
        zombie.Setup(zombieData);

        // 생성한 좀비를 좀비 리스트에 추가하여 관리함
        zombies.Add(zombie);

        // 좀비의 onDeath 이벤트(델리게이트 타입 변수) 에 익명함수를 만들어서 이벤트 리스너로 등록함
        // 사망한 좀비를 좀비 리스트에서 제거하는 익명함수를 이벤트 리스너로 등록
        zombie.onDeath += () => zombies.Remove(zombie);
        // 사망한 좀비 컴포넌트의 게임 오브젝트를 10초 뒤에 파괴하는 익명함수를 이벤트 리스너로 등록
        zombie.onDeath += () => Destroy(zombie.gameObject, 10f);
        // 좀비 사망 시, 게임매니저 싱글턴 인스턴스를 통해 점수 데이터 추가 메서드를 실행하는 익명함수를 이벤트 리스너로 등록
        zombie.onDeath += () => GameManager.instance.AddScore(100);
    }
}