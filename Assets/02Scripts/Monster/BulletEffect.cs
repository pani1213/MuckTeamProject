using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    public GameObject destroyEffectPrefab; // �Ѿ��� ������ �� ������ ����Ʈ ������

    // �Ѿ��� ���� �� ȣ��Ǵ� �Լ�
    public void DestoryEffect()
    {
        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }
    }

}
