using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Item,
    Object_Onley,
    build,
    box,
    brazier
}
public class ItemObjectScript : MonoBehaviour 
{
    public int id;
    public int count = 1;
    public ItemType Item_type;
    public void InIt(int _itemId, int _count, ItemType _itemType)
    {
        if (_itemType == ItemType.Item)
        { 
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }
        Item_type = _itemType;
        id = _itemId;
        count = _count;
    }
    public void InIt(ItemType _itemType)
    {
        Item_type = _itemType;
        if (_itemType == ItemType.Item)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }
        if (_itemType == ItemType.build || _itemType == ItemType.brazier || _itemType == ItemType.box)
        {
            if (_itemType == ItemType.box)
                gameObject.tag = "Box";

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
        }
      
    }
    public void GetItem()
    {
        if (Item_type == ItemType.Item)
        {
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[id], count);
            gameObject.SetActive(false);
        }
        if (Item_type == ItemType.build)
        {
            BuildManager.instance.buildController.InIt(id);
        }
    }
}

