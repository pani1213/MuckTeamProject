using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Animator animator;
    public BoxCollider BoxCollider;
    public GameObject AttachPosition;
    public InvenItem AttachItem = null;

    private int currentIndex = 0;
    public static int attachmentDamage;
    private Vector3 startPos;
    float coolTime = 0;
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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        { 
            AttachMentItem(21);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        { 
            AttachMentItem(22);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        { 
            AttachMentItem(23);
        }
        if (AttachItem != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (AttachItem.item.category == "food")
                {
                    FoodActionCoolTime(1);
                }
                else if (AttachItem.item.category == "tool")
                {
                    StartCoroutine(Attack_Coroutione());
                }
            }
        }
    }
    public void FoodActionCoolTime(float _value)
    {
        coolTime += Time.deltaTime;
        if (coolTime > _value)
        {
            Debug.Log("Event");
            SurvivalGauge.Instance.PlayerHealth += ItemInfoManager.instance.itemInventory[currentIndex].item.value[0];
            SurvivalGauge.Instance.PlayerHunger += ItemInfoManager.instance.itemInventory[currentIndex].item.value[1];
            SurvivalGauge.Instance.Stamina += ItemInfoManager.instance.itemInventory[currentIndex].item.value[2];
            
            if (--ItemInfoManager.instance.itemInventory[currentIndex].count <= 0)
                AttachItem = null;
            
            ItemInfoManager.instance.RefreshQuickSlots();
            if (AttachPosition.transform.childCount > 0)
                Destroy(AttachPosition.transform.GetChild(0).gameObject);
            coolTime = 0;
        }
    }
    IEnumerator Attack_Coroutione()
    {
        BoxCollider.enabled = true;
        yield return new WaitForSeconds(PlayerFireAbility.Instance.AttackSpeed);
        BoxCollider.enabled = false;
    }
    public void AttachMentItem(int _itemIndex)
    {
        AttachItem = null;
        currentIndex = 0;
        attachmentDamage = 0;
        if (AttachPosition.transform.childCount > 0)
            Destroy(AttachPosition.transform.GetChild(0).gameObject);

        if (ItemInfoManager.instance.itemInventory[_itemIndex].item != null)
        { 
            AttachItem = ItemInfoManager.instance.itemInventory[_itemIndex];
            attachmentDamage = ItemInfoManager.instance.itemInventory[_itemIndex].item.damage;
            currentIndex = _itemIndex;
              GameObject obj = Instantiate(GetItemPrefab(ItemInfoManager.instance.itemInventory[_itemIndex].item.id.ToString()), AttachPosition.transform);
            obj.transform.localPosition = Vector3.zero;
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
