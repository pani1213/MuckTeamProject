using System.Collections.Generic;
using UnityEngine;

public class BulletEffectPool : MonoBehaviour
{
    public GameObject BulletDestroyEffectPrefab; // �Ѿ� ���� ����Ʈ ������
    public int initialPoolSize = 10; // �ʱ� Ǯ ũ��
    private Queue<GameObject> effectPool = new Queue<GameObject>(); // ����Ʈ Ǯ

    private void Start()
    {
        // �ʱ� Ǯ ũ�⸸ŭ ����Ʈ ������Ʈ�� �����Ͽ� Ǯ�� �߰�
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject effectInstance = Instantiate(BulletDestroyEffectPrefab);
            effectInstance.SetActive(false);
            effectPool.Enqueue(effectInstance);
        }
    }

    // ����Ʈ�� �������� �Լ�
    public GameObject GetEffect()
    {
        if (effectPool.Count == 0)
        {
            // Ǯ�� ����Ʈ�� ���� ��� ���� �����Ͽ� ��ȯ
            GameObject effectInstance = Instantiate(BulletDestroyEffectPrefab);
            return effectInstance;
        }
        else
        {
            // Ǯ���� ����Ʈ�� ������ ��ȯ
            GameObject effectInstance = effectPool.Dequeue();
            effectInstance.SetActive(true);
            return effectInstance;
        }
    }

    // ����Ʈ�� Ǯ�� ��ȯ�ϴ� �Լ�
    public void ReturnEffect(GameObject effect)
    {
        effect.SetActive(false);
        effectPool.Enqueue(effect);
    }
}
