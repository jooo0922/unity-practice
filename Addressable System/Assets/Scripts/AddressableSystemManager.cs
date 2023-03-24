using System.Collections;
using System.Collections.Generic;
using System; // 익명함수 콜백을 등록하기 위한 Action 타입 변수 선언을 위해 선언된 네임스페이스 
using UnityEngine;
using UnityEngine.AddressableAssets; // 어드레서블 시스템 API 사용을 위해 선언된 네임스페이스
using UnityEngine.ResourceManagement.AsyncOperations; // 비동기 처리 사용을 위해 선언된 네임스페이스
using UnityEngine.ResourceManagement.ResourceLocations; // IResourceLocation 타입을 사용하기 위해 선언된 네임스페이스

public class AddressableSystemManager : MonoBehaviour
{
    private AudioSource audioSource;

    private List<Sprite> allIconSprites = new List<Sprite>(); // 스프라이트 타입의 어드레서블 에셋을 로드해와서 모아둘 리스트

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMainMusicRoutine());
        StartCoroutine(LoadAllWeaponsRoutine(true));

        // 비동기로 프리팹 에셋을 가져와서 게임 오브젝트 복사본으로 인스턴스화시키는 오퍼레이션 핸들을 반환하는 어드레서블 API 
        var operationHandle = Addressables.InstantiateAsync("monster_red");
        operationHandle.Completed += handle =>
        {
            GameObject createdMonster = handle.Result;
            Debug.Log($"몬스터가 생성됨! {createdMonster.name}"); // JS 의 템플릿 리터럴과 유사한 c#의 문자열 보간 문법
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 코루틴 메서드로 정의 -> why? 에셋을 비동기로 로드하는 동안 대기시간을 주려고
    private IEnumerator PlayMainMusicRoutine()
    {
        AsyncOperationHandle<AudioClip> operationHandle = Addressables.LoadAssetAsync<AudioClip>("main_music");

        yield return operationHandle; // 에셋을 가져오는 오퍼레이션을 기다리기 -> 코루틴 메서드 사용 이유
        AudioClip musicClip = operationHandle.Result; // 비동기 오퍼레이션 결과값인 오디오 클립 가져오기

        // 오디오 소스를 통해 재생
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    // 코루틴 메서드로 여러 에셋을 비동기로 가져오기
    private IEnumerator LoadAllWeaponsRoutine(bool useHighDefinition)
    {
        List<string> keys = new List<string>(); // Addressables.LoadAssetsAsync<T>() 의 첫 번째 인자로 전달할 어드레스 또는 레이블 키 콜렉션
        keys.Add("weapons"); // 일단 로드하려는 에셋은 "weapons" 어드레스를 반드시 가져야 함.

        // 고해상도 모델 사용 여부 입력값에 따라 "HD" 어드레스를 갖는 에셋들을 로드할 지, "SD" 어드레스를 갖는 에셋들을 로드할 지 결정
        if (useHighDefinition)
        {
            keys.Add("HD");
        }
        else
        {
            keys.Add("SD");
        }

        // Addressables.LoadAssetsAsync<T>() 의 두 번째 인자로 전달할 에셋이 로드될 때마다 호출될 콜백 익명함수
        Action<GameObject> callback = (GameObject loadedAsset) =>
        {
            Debug.Log($"로드된 에셋 이름 : {loadedAsset.name}");
        };

        AsyncOperationHandle<IList<GameObject>> operationHandle =
            Addressables.LoadAssetsAsync<GameObject>(
                keys,
                callback,
                Addressables.MergeMode.Intersection // 세 번째 인자는 첫 번째 인자로 전달한 키 리스트를 어떻게 엮어서 에셋 로드 조건을 만들지 결정함 -> Intersection 은 키 리스트를 전부 갖고있는 에셋들만 로드함.
            );

        yield return operationHandle; // 에셋들을 가져오는 오퍼레이션 기다리기

        IList<GameObject> loadedWeaponPrefabs = operationHandle.Result; // 비동기 오퍼레이션 결과값인 무기 게임 오브젝트 리스트를 변수에 할당

        // 로드된 무기 loadedWeaponPrefabs 을 사용하는 나머지 코드
    }

    // 코루틴 메서드로 어드레스(키)에 대응하는 IResourceLocation 리스트를 비동기로 로드하기
    // 거기에는 키에 대응하는 에셋들의 위치와 정보가 있는데, 에셋의 타입이 스프라이트면 해당 에셋을 실제로 비동기 로드해오는 로직까지 추가
    private IEnumerator LoadAllIconSprites()
    {
        List<string> keys = new List<string>() { "ui", "icon" };
        AsyncOperationHandle<IList<IResourceLocation>> locationOperationHandle =
            Addressables.LoadResourceLocationsAsync(keys); // IResourceLocation 객체를 비동기로 로드하는 오퍼레이션 핸들을 반환.

        yield return locationOperationHandle;

        IList<IResourceLocation> locations = locationOperationHandle.Result; // 위의 키 목록에 대응하는 IResourceLocation 리스트를 비동기로 가져옴.

        // IResourceLocation 리스트를 사용하는 예시 -> 특정 타입의 에셋에 해당할 경우 실제 에셋을 비동기로 로드해 옴.
        foreach (var location in locations)
        {
            // IResourceLocation 객체에 명시된 리소스 타입이 Sprite 일 경우애만 실제 에셋을 비동기로 로드해 옴.
            if (location.ResourceType == typeof(Sprite))
            {
                AsyncOperationHandle<Sprite> loadOperationHandle =
                    Addressables.LoadAssetAsync<Sprite>(location); // Addressable.LoadAssetAsync() 메서드는 IResourceLocation 타입도 그대로 인자로 전달할 수 있음.
                yield return loadOperationHandle; // 실제 비동기로 스프라이트를 로드함
                allIconSprites.Add(loadOperationHandle.Result); // 로드한 스프라이트 에셋을 리스트 멤버변수에 추가함.
            }
        }
    }
}
