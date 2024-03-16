using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButtonSlot : MonoBehaviour
{
    public int makeId;
    List<int> indexs;
    public Image images;
    public void ButtonAction_CreateButtonSlot()
    {
        indexs= new List<int>();
        // ������ ���� �� �ִ��� Ȯ��, �����
        bool isMake = true;
        Item item = JsonParsingManager.instance.ItemDic[makeId];
        for (int i = 0; i < item.makeResource.Length; i++)
        {
            indexs.Add(ItemInfoManager.instance.GetItemIndex(JsonParsingManager.instance.ItemDic[item.makeResource[i]]));
            if (indexs[i] == -1)
            { 
                isMake = false;
                continue;
            }
            if (ItemInfoManager.instance.itemInventory[indexs[i]].count < item.makeResourceCount[i])
                isMake = false;
        }
        if (isMake)
        {
            for (int i = 0; i < indexs.Count; i++)
                ItemInfoManager.instance.itemInventory[indexs[i]].count -= item.makeResourceCount[i];
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[makeId], JsonParsingManager.instance.ItemDic[makeId].makeCount);

            ItemInfoManager.instance.inventoryController.RefreshAllSlot();
            Debug.Log("������ ����");
        }
        else
            Debug.Log("��������");
    }
}