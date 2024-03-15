using System.Collections.Generic;
using UnityEngine;

public class BulletEffectPool : MonoBehaviour
{
    public GameObject BulletDestroyEffectPrefab; // 총알 삭제 이펙트 프리팹
    public int initialPoolSize = 10; // 초기 풀 크기
    private Queue<GameObject> effectPool = new Queue<GameObject>(); // 이펙트 풀

    private void Start()
    {
        // 초기 풀 크기만큼 이펙트 오브젝트를 생성하여 풀에 추가
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject effectInstance = Instantiate(BulletDestroyEffectPrefab);
            effectInstance.SetActive(false);
            effectPool.Enqueue(effectInstance);
        }
    }

    // 이펙트를 가져오는 함수
    public GameObject GetEffect()
    {
        if (effectPool.Count == 0)
        {
            // 풀에 이펙트가 없는 경우 새로 생성하여 반환
            GameObject effectInstance = Instantiate(BulletDestroyEffectPrefab);
            return effectInstance;
        }
        else
        {
            // 풀에서 이펙트를 꺼내서 반환
            GameObject effectInstance = effectPool.Dequeue();
            effectInstance.SetActive(true);
            return effectInstance;
        }
    }

    // 이펙트를 풀에 반환하는 함수
    public void ReturnEffect(GameObject effect)
    {
        effect.SetActive(false);
        effectPool.Enqueue(effect);
    }
}
