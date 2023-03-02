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

    }
}