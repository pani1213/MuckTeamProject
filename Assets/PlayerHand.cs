using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Animator animator;
    public BoxCollider BoxCollider;
    public GameObject AttachPosition;
    public InvenItem AttachItem = null;

    public int currentIndex = 0;
    public static int attachmentDamage;
    private Vector3 startPos;
    float coolTime = 0;
    RaycastHit hit;
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
        if (hit.collider == null)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    Debug.Log("item get");
                    hit.collider.gameObject.GetComponent<ItemObjectScript>().GetItem();
                }
                if (hit.collider.CompareTag("BoxItem"))
                {
                    Debug.Log("boxItem get");
                    hit.collider.gameObject.GetComponent<RandomBoxItem>().GetItem();
                }
            }
            if (AttachItem != null &&    AttachItem.item.category == "build")
            {
                BuildManager.instance.GetObject(AttachItem.item.id);
                BuildManager.instance.GetObject(AttachItem.item.id).transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                BuildManager.instance.GetObject(AttachItem.item.id).transform.LookAt(transform);

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject gameobj = Instantiate(ItemInfoManager.instance.itemdic[AttachItem.item.id].gameObject);
                    gameobj.transform.SetPositionAndRotation(BuildManager.instance.GetObject(AttachItem.item.id).transform.position,
                        BuildManager.instance.GetObject(AttachItem.item.id).transform.rotation);

                    if (--ItemInfoManager.instance.itemInventory[currentIndex].count <= 0)
                        AttachItem = null;

                    ItemInfoManager.instance.RefreshQuickSlots();
                    if (AttachPosition.transform.childCount > 0)
                        Destroy(AttachPosition.transform.GetChild(0).gameObject);
                    // 상자 생성, 위치 hit.pointer
                }
            }
            if (AttachItem == null || AttachItem.item.category != "build")
                BuildManager.instance.ReturnObject();
        }
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10, out hit);
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
