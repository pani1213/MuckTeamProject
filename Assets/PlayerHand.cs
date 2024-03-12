using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Animation animation;
    public BoxCollider BoxCollider;
    public GameObject AttachPosition;
    public InvenItem AttachItem = null;
    public static int attachmentDamage;
    private Vector3 startPos;
    private void Start()
    {
        startPos = AttachPosition.transform.position; 
    }
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
        if (AttachItem != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (AttachItem.item.category == "food")
                {
                    Debug.Log(AttachItem.item.category);
                    animation.Play("PlayerEat");
                }
                if (AttachItem.item.category == "tool")
                {
                    Debug.Log(AttachItem.item.category);
                    animation.Play("swingClip");
                    StartCoroutine(Attack_Coroutione());
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (AttachItem.item.category == "food")
                {
                    Debug.Log("GripIdle play");
                    animation.Play("GripIdle");
                }
            }
        }

    }
    IEnumerator Attack_Coroutione()
    {
        BoxCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        BoxCollider.enabled = false;
    }
    public void AttachMentItem(int _itemIndex)
    {
        if (ItemInfoManager.instance.itemInventory[_itemIndex] != AttachItem  && AttachPosition.transform.childCount > 0)
            Destroy(AttachPosition.transform.GetChild(0).gameObject);

        if (ItemInfoManager.instance.itemInventory[_itemIndex].item != null)
        {
            AttachItem = ItemInfoManager.instance.itemInventory[_itemIndex];
            attachmentDamage = ItemInfoManager.instance.itemInventory[_itemIndex].item.damage;
            Debug.Log(ItemInfoManager.instance.itemInventory[_itemIndex].item.id);
            GameObject obj = Instantiate(GetItemPrefab(ItemInfoManager.instance.itemInventory[_itemIndex].item.id.ToString()), AttachPosition.transform);
            obj.transform.localPosition = Vector3.zero; 
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.layer = 2;
            Debug.Log(obj.layer);
        }
        else
        {
            AttachItem = null;
            Debug.Log("isNull");
        }
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
