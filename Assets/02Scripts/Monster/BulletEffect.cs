using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    public GameObject destroyEffectPrefab; // 총알이 없어질 때 생성될 이펙트 프리팹

    // 총알이 박힐 때 호출되는 함수
    public void DestoryEffect()
    {
        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }
    }

}
