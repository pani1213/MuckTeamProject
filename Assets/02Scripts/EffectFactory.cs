
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{
    public static EffectFactory Instance { get; private set; }

    // (������) ������ �����յ�
    public List<GameObject> EffectPrefabs;

    // ������ â��
    private List<EffectFactory> _effectPool;
    public int PoolCount = 10;

    public ItemType ItemType;
    

    public void Awake()
    {

        // �̱��� �ν��Ͻ��� ����
        Instance = this;

        // ������ Ǯ�� �ʱ�ȭ
        _effectPool = new List<EffectFactory>();


        // PoolCount ��ŭ �������� �����Ͽ� Ǯ�� ����
        for (int i = 0; i < PoolCount; ++i)             // 10��
        {
            foreach (GameObject prefab in EffectPrefabs)  // 3��
            {
                // 1. �����
                GameObject item = Instantiate(prefab);
                // 2. â�� �ִ´�.
                item.transform.SetParent(this.transform);
                _effectPool.Add(item.GetComponent<EffectFactory>());
                // 3. ��Ȱ��ȭ
                item.SetActive(false);
            }

        }
    }

    // Get �޼ҵ�� �־��� ������ Ÿ�Կ� �ش��ϴ� ������ ������Ʈ�� ������ Ǯ���� ã�� ��ȯ
    private EffectFactory Get(ItemType itemType) // â�� ������ 
    {
        foreach (EffectFactory itemObject in _effectPool) // â�� ������.
        {
            if (itemObject.gameObject.activeSelf == false && itemObject.ItemType == itemType)
            // activeSelf: GameObject�� ���� Ȱ��ȭ�Ǿ� �ִ��� �ƴ��� bool�� 
            {
                return itemObject;
            }
        }

        return null;
    }

    // MakePercent �޼ҵ�: Ȯ�� ���� (�����! �����ڽ� �ֹ��Ұ�!)
    public void MakePercent(Vector3 position)
    {
        int percentage = UnityEngine.Random.Range(0, 100);
        if (percentage <= 20)
        {
            //Make(ItemType.Health, position);
        }
        else if (percentage <= 40)
        {
            //Make(ItemType.Stamina, position);
        }
        else if (percentage <= 50)
        {
            //Make(ItemType.Bullet, position);
        }
    }

    // Make �޼ҵ�: �⺻ ���� (�����! ���� ���ϴ°� �ֹ��Ұ�!)
    public void Make(ItemType itemType, Vector3 position)
    {
        // ������ Ǯ���� ������ ������Ʈ�� ������
        //ItemObject itemObject = Get(itemType);

        /*if (itemObject != null)
        {
            // �������� ��ġ�� �����ϰ� �������� Ȱ��ȭ
            itemObject.transform.position = position;
            itemObject.Init();
            itemObject.gameObject.SetActive(true);
        }*/
    }

    // � ȿ���� ������ �� ���ΰ� ���Ŀ� �� �� �ִ� �͵�
    //private IEnumerator Effect_Coroutine()
    //{
    /* while (_progress < 0.8f)
     {
         // ���൵�� �����ϴ� �ð��� ����ϰ�, ���� ��ġ�� �����մϴ�.
         _progress += Time.deltaTime / TRACE_DURATION;
         transform.position = Vector3.Slerp(_startPosition, _player.position, _progress);

         // ���� �����ӱ��� ����Ѵ�.
         yield return null;
     }

     // ���൵�� 0.8 �̻��̸� �������� �߰��ϰ� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
     ItemManager.Instance.AddItem(ItemType);
     gameObject.SetActive(false);

 }*/
}
