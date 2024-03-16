using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    public GameObject bulletEffectPrefab; // 총알 이펙트 프리팹
    public float DestroyTime = 1f;
    public void CreateBulletEffect(Vector3 position)
    {
        if (bulletEffectPrefab != null)
        {
            GameObject effectInstance = Instantiate(bulletEffectPrefab, position, Quaternion.identity);
       

            // 일정 시간이 지난 후에 이펙트를 비활성화합니다.
            Destroy(effectInstance, DestroyTime); 
        }
    }
}
