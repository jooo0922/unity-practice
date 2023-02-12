using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMax = 2.25f; // 다음 배치까지의 시간 간격 최댓값
    private float timeBetSpawn; // 다음 배치까지의 시간 간격

    public float yMin = -3.5f; // 배치할 위치의 최소 y값
    public float yMax = 1.5f; // 배치할 위치의 최대 y값
    private float xPos = 20f; // 배치할 위치의 x 값

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; // 마지막 배치 시점


    void Start() {
        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        // count 만큼의 공간을 갖는 발판 게임 오브젝트를 담아둘 배열 생성
        platforms = new GameObject[count];

        // count 만큼 루프를 돌면서 오브젝트 풀에 담아둘 발판 게임 오브젝트 생성 및 platforms 배열에 할당
        for (int i = 0; i < count; i++)
        {
            // 발판 원본 프리팹으로부터 복제된 게임 오브젝트 인스턴스를 생성하여 배열에 추가
            // 참고로, 복제된 게임 오브젝트의 회전값을 설정하는 세 번째 인자는 쿼터니언 타입이 들어가야 함.
            // 근데, Quaternion.identity 는 뭘까? 이거는 오일러 각으로 변환하면 (0, 0, 0) 회전값과 동일하다고 함!
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        // 맨 처음 배치되는 발판은 지연시간 없이 즉시 배치하기 위해 최근 재배치 시점과 배치 시간간격 변수를 모두 0으로 초기화함
        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    void Update() {
        // 순서를 돌아가며 주기적으로 발판을 배치
        // 게임오버 상태에서는 동작하지 않도록, 싱글턴 GameManager 오브젝트의 instance 멤버변수로부터 게임오버 상태값 검사
        if (GameManager.instance.isGameover)
        {
            return;
        }

        // 현재 시점이 (마지막 발판 배치 시점 + 랜덤한 발판 배치 시간 간격) 과 같거나 그것보다 지났으면 발판 재배치 로직 실행
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 마지막 발판 배치 시점을 현재 시점으로 업데이트 (이제 발판을 새로 배치할거니까!)
            lastSpawnTime = Time.time;

            // 다음 발판 배치 시간 간격을 랜덤하게 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            // 발판 배치 높이 랜덤 설정
            float yPos = Random.Range(yMin, yMax);

            // 이번에 배치할 순번에 해당하는 발판 게임 오브젝트를 Start() 메서드에서 생성한 발판 게임 오브젝트 배열에서 가져온 뒤,
            // 게임 오브젝트 비활성화 > 다시 활성화. 즉, 게임 오브젝트를 껏다 킴.
            // 이로 인해 발판 게임 오브젝트의 Platform 스크립트 컴포넌트가 따라서 비활성화 > 다시 활성화 되면서
            // OnEnable() 이벤트 메서드가 실행됨 -> 발판 게임 오브젝트의 상태를 리셋시킴!
            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            // 이번에 배치할 순번의 발판 게임 오브젝트를 화면 오른쪽에 재배치 (재배치 높이값은 랜덤!)
            platforms[currentIndex].transform.position = new Vector2(xPos, yPos);

            // 재배치할 발판 게임 오브젝트의 순번을 증가시켜서 다음 재배치할 때 사용하도록 함.
            currentIndex++;

            // 재배치할 발판 게임 오브젝트의 순번이 마지막 순번에 도달했다면, 맨 첫 번째 순번인 0번으로 초기화함
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}