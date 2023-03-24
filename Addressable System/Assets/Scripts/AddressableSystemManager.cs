using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; // 어드레서블 시스템 API 사용을 위해 선언된 네임스페이스
using UnityEngine.ResourceManagement.AsyncOperations; // 비동기 처리 사용을 위해 선언된 네임스페이스

public class AddressableSystemManager : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMainMusicRoutine());
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
}
