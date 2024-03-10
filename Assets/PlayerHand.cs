using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public GameObject AttachPosition;
    public Item AttachItem = null;

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AttachMentItem(18);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AttachMentItem(19);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AttachMentItem(20);
        }
    }
    public void AttachMentItem(int _itemIndex)
    {
        if (ItemInfoManager.instance.itemInventory[_itemIndex].item != null)
        {
            Debug.Log(ItemInfoManager.instance.itemInventory[_itemIndex].item.id);
            GameObject obj =  Instantiate(GetItemPrefab(ItemInfoManager.instance.itemInventory[_itemIndex].item.id.ToString()), AttachPosition.transform);
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.layer = 2;
            Debug.Log(obj.layer);
        }
        else
            Debug.Log("isNull");
    }
    private GameObject GetItemPrefab(string _id)
    {
        for (int i = 0;i < ItemInfoManager.instance.ItemPrefabs.Count; i++)
        {
            if (ItemInfoManager.instance.ItemPrefabs[i].name == _id)
                return ItemInfoManager.instance.ItemPrefabs[i].gameObject;
        }
        return null;
    }
}