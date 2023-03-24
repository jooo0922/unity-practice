using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // 익명함수 콜백을 등록하기 위한 Action 타입 변수 선언을 위해 선언된 네임스페이스 
using UnityEngine.AddressableAssets; // 어드레서블 시스템 API 사용을 위해 선언된 네임스페이스
using UnityEngine.ResourceManagement.AsyncOperations; // 비동기 처리 사용을 위해 선언된 네임스페이스

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AssetReference musicClipReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        // AssetReference 타입으로부터 에셋을 가져올 때에는, 별도의 어드레스 문자열을 입력하지 않고 바로 LoadAssetAsync() 를 호출하면 됨.
        // 왜냐, 이미 AssetReference 타입 변수에 GUID 가 할당되어 있으니까!
        AsyncOperationHandle<AudioClip> operationHandle =
            musicClipReference.LoadAssetAsync<AudioClip>();

        operationHandle.Completed += (operation) =>
        {
            AudioClip audioClip = operation.Result;
            audioSource.clip = audioClip;
            audioSource.Play();
        };
    }
}
