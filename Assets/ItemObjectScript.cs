using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Item,
    Object_Onley
}
public class ItemObjectScript : MonoBehaviour 
{
    public int id;
    public int count;
    public ItemType Item_type;
    public void InIt(int _itemId, int _count)
    {
        id = _itemId;
        count = _count;
    }
    public void GetItem()
    {
        if (Item_type == ItemType.Item)
        {
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[id], count);
            gameObject.SetActive(false);
        }

    }
}

