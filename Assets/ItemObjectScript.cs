using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectScript : MonoBehaviour 
{
    public int id;
    public int count;

    public void InIt(int _itemId, int _count)
    {
        id = _itemId;
        count = _count;
    }
    public void GetItem()
    {
        ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[id], count);
    }
}

