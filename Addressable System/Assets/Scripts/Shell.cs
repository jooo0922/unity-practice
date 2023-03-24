using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // 익명함수 콜백을 등록하기 위한 Action 타입 변수 선언을 위해 선언된 네임스페이스 
using UnityEngine.AddressableAssets; // 어드레서블 시스템 API 사용을 위해 선언된 네임스페이스
using UnityEngine.ResourceManagement.AsyncOperations; // 비동기 처리 사용을 위해 선언된 네임스페이스

public class Shell : MonoBehaviour
{
    public AssetReference explosionPrefabReference; // 에셋 레퍼런스 타입 멤버변수에는 실제 어드레서블 에셋이 아닌, 해당 에셋을 찾는데 필요한 GUID 가 할당됨.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 다른 콜라이더와 충돌했을 때, 충돌 이벤트 메서드에서 폭발 효과 프리팹 에셋을 가져와서 인스턴스화하고, 파티클 시스템 컴포넌트를 재생함
    private void OnTriggerEnter(Collider other)
    {
        /* 충돌한 상대방 게임 오브젝트에 대미지를 주는 처리... */

        // 폭발 효과 프리팹을 어드레서블 시스템 API 를 통해 비동기 로드 후 인스턴스화함.
        // AssetReference.InstantiateAsync() 에서는 입력 파라미터로 인스턴스의 position, rotation 를 전달함. 
        AsyncOperationHandle<GameObject> operationHandle =
            explosionPrefabReference.InstantiateAsync(
                transform.position, Quaternion.identity);

        // 인스턴스화가 완료되었을 때의 콜백함수 등록 -> 인스턴스화된 폭발효과 게임 오브젝트의 파티클 시스템 컴포넌트 재생
        operationHandle.Completed += (handle) =>
        {
            GameObject explosionGameObject = handle.Result;
            ParticleSystem explosionParticleSystem =
                explosionGameObject.GetComponent<ParticleSystem>();
            explosionParticleSystem.Play();
        };
        Destroy(gameObject); // Shell 스크립트 컴포넌트가 추가되어 있는 Shell 게임 오브젝트, 즉 포탄 게임 오브젝트 자신을 파괴
    }

}
