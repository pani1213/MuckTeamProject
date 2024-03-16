using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    public GameObject bulletEffectPrefab; // �Ѿ� ����Ʈ ������
    public float DestroyTime = 1f;
    public void CreateBulletEffect(Vector3 position)
    {
        if (bulletEffectPrefab != null)
        {
            GameObject effectInstance = Instantiate(bulletEffectPrefab, position, Quaternion.identity);
       

            // ���� �ð��� ���� �Ŀ� ����Ʈ�� ��Ȱ��ȭ�մϴ�.
            Destroy(effectInstance, DestroyTime); 
        }
    }
}
