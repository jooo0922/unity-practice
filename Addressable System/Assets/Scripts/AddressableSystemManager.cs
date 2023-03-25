using System.Collections;
using System.Collections.Generic;
using System; // 익명함수 콜백을 등록하기 위한 Action 타입 변수 선언을 위해 선언된 네임스페이스 
using UnityEngine;
using UnityEngine.AddressableAssets; // 어드레서블 시스템 API 사용을 위해 선언된 네임스페이스
using UnityEngine.AddressableAssets.ResourceLocators;
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
        StartCoroutine(LoadAllIconSprites());
        StartCoroutine(loadMusicRoutine());
        StartCoroutine(UpdateCatalogsRoutine());

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

    // 코루틴 메서드로 어드레스(키)에 대응하는 IResourceLocation 리스트를 비동기로 로드하기
    // 키에 대응하는 IResourceLocation 가 존재하지 않는 경우, 해당 어드레스를 갖고 있는 어드레서블 에셋이 존재하지 않음을 검사할 수 있음.
    private IEnumerator loadMusicRoutine()
    {
        string key = "main_music";
        AsyncOperationHandle<IList<IResourceLocation>> operationHandle =
                Addressables.LoadResourceLocationsAsync(key);

        yield return operationHandle;

        IList<IResourceLocation> locations = operationHandle.Result;

        // IResourceLocation 리스트를 사용하는 예시
        // IResourceLocation 리스트 개수가 0이라면, 상단의 어드레스 키에 대응되는 어드레서블 에셋이 존재하지 않는다는 것을 검사할 수 있음.
        if (locations.Count <= 0)
        {
            Debug.Log("키에 대응하는 어드레서블 에셋이 없음");
        }
        else
        {
            // main_music 어드레서블 에셋을 실제 로드하는 다른 처리
        }
    }

    // 코루틴 메서드에서 어드레스에 해당하는 어드레서블 에셋과 그것의 의존성 에셋을 전부 원격서버에서 다운로드하기
    public IEnumerator PreloadDownloadEssentialAssetsRoutine()
    {
        // 필수 어드레서블 에셋에 태그된 "essential" 레이블로 검색할 예정
        string essentialAssetLabel = "essential";

        // 해당 키에 대응하는 에셋과 의존성 에셋의 다운로드 크기 가져옴
        AsyncOperationHandle<long> getDownloadSizeOperation =
            Addressables.GetDownloadSizeAsync(essentialAssetLabel);

        yield return getDownloadSizeOperation; // 다운로드 크기를 비동기로 로드해오는 오퍼레이션 수행을 대기함

        // 실제 비동기로 가져온 다운로드 크기를 64비트 정수타입(long) 변수에 저장
        long downloadSize = getDownloadSizeOperation.Result;

        // 더 이상 가져오려는 에셋의 다운로드 크기를 비동기로 로드해올 필요가 없으니, 메모리 절약을 위해 오퍼레이션 핸들을 해제함.
        Addressables.Release(getDownloadSizeOperation);

        // 다운로드 크기가 0보다 크다면, 즉, 다운로드해야 할 에셋이 존재한다면, 실제 비동기 다운로드 수행
        if (downloadSize > 0)
        {
            Debug.Log($"필수 에셋들을 다운로드합니다. 다운로드 크기 : {downloadSize}");

            // 해당 에셋과 의존성 에셋을 모두 다운로드하는 오퍼레이션 핸들 반환
            AsyncOperationHandle downloadDependenciesHandle =
                Addressables.DownloadDependenciesAsync(essentialAssetLabel);

            // 의존성 다운로드 오퍼레이션이 진행되는 동안 매 프레임마다 다운로드 정보 데이터인 DownloadStatus 를 가져와서 디버그 창에 표시
            while (!downloadDependenciesHandle.IsDone)
            {
                // 현재 다운로드 상태가 담긴 DownloadStatus 타입 오브젝트 가져오기
                DownloadStatus downloadStatus =
                    downloadDependenciesHandle.GetDownloadStatus();
                Debug.Log($"다운로드해야 할 전체 크기 : {downloadStatus.TotalBytes}"); // 전체 다운로드 용량
                Debug.Log($"현재 다운로드된 크기 : {downloadStatus.DownloadedBytes}"); // 현재까지 다운로드된 용량
                Debug.Log($"진행도 : {downloadStatus.Percent}"); // 현재 다운로드 용량 / 전체 다운로드 용량의 비율이겠지?
                yield return null; // yield 문으로 의존성 다운로드 오퍼레이션이 완료될 때까지 while 반복문을 매 프레임마다 실행토록 함
            }

            // 이제 의존성 다운로드 오퍼레이션이 완료되었기 때문에, 해당 오퍼레이션을 더 이상 실행하지 않아도 되므로, 메모리를 해제시킴.
            Addressables.Release(downloadDependenciesHandle);
            Debug.Log("다운로드 끝!");
        }
    }

    // 코루틴 메서드에서 앱에 캐싱되어 있는 기본 카탈로그를 업데이트하기
    private IEnumerator UpdateCatalogsRoutine()
    {
        // 원격 서버의 카탈로그 파일을 다운로드해서 앱 내부에 캐싱된 전체 카탈로그들을 업데이트함. (카탈로그 이름 컬렉션을 입력하지 않았으므로, 전체 카탈로그가 업데이트됨.)
        AsyncOperationHandle<List<IResourceLocator>> updateCatalogsOperationHandle =
            Addressables.UpdateCatalogs(autoReleaseHandle: false);

        yield return updateCatalogsOperationHandle; // 카탈로그 업데이트 오퍼레이션 핸들 실행 대기
        // .Result 에는 업데이트된 각 카탈로그들을 참조해서 IResourceLocation 객체를 만드는 리소스 로케이터들이 저장된 리스트가 담겨있음!

        Addressables.Release(updateCatalogsOperationHandle); // 카탈로그 업데이트 오퍼레이션 핸들 메모리 해제
    }
}
