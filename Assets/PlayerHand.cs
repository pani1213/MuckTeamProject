using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHand : MonoBehaviour
{
    public Text informationText_UI;
    public Animator animator;
    public BoxCollider BoxCollider;
    public GameObject AttachPosition;
    public InvenItem AttachItem = null;

    public int currentIndex = 0;
    public static int attachmentDamage;
    float foodCoolTime = 0;
    RaycastHit hit;

    public bool isAttackDiley = true;
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
                    animator.SetBool("IsEat", true);
                    FoodActionCoolTime(1);
                }
                else if (AttachItem.item.category == "tool" || AttachItem.item.type == 3)
                {
                    animator.SetBool("IsSwing", true);
                    if(isAttackDiley)
                    StartCoroutine(Attack_Coroutione());
                }
            }
            else
            {
                animator.SetBool("IsSwing", false);
                animator.SetBool("IsEat", false);
                foodCoolTime = 0;
            }
        }
        if (hit.collider == null)
        {
            informationText_UI.text = "";
        }
        else
        {
            if (hit.collider.CompareTag("Item"))
                informationText_UI.text = "Prees 'E' key Get Item";
            else if (hit.collider.CompareTag("InvenBox"))
                informationText_UI.text = "Prees 'E' key Open the Box";
            else
                informationText_UI.text = "";
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    hit.collider.gameObject.GetComponent<ItemObjectScript>().GetItem();
                }
                if (hit.collider.CompareTag("BoxItem"))
                {
                    Debug.Log("boxItem get");
                    //hit.collider.gameObject.GetComponent<RandomBoxItem>().GetItem();
                }
                if (hit.collider.CompareTag("InvenBox"))
                {
                    hit.collider.gameObject.GetComponent<BoxObjectScript>().OpenBoxAction();
                    Debug.Log("boxItem get");
                }
            }
            if (AttachItem != null && AttachItem.item.category == "build")
            {
                GameObject buildObj = BuildManager.instance.GetObject(AttachItem.item.id);
                buildObj.transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                buildObj.transform.LookAt(transform);
                if (Input.GetMouseButtonDown(0))
                {
                    ItemObjectScript itemObj = Instantiate(ItemInfoManager.instance.itemdic[AttachItem.item.id]);

                    if (AttachItem.item.id == 1018) // box ¿œ∂ß
                    { 
                        itemObj.InIt(ItemType.box);
                        itemObj.GetComponent<BoxObjectScript>().InIt();
                    }
                    if (AttachItem.item.id == 1020 || AttachItem.item.id == 1019)
                        itemObj.InIt(ItemType.build);
                    if (AttachItem.item.id == 1021)
                    { 
                        itemObj.InIt(ItemType.brazier);
                        itemObj.GetComponent<BrazierObject>().InIt();
                    }

                    itemObj.transform.SetPositionAndRotation(buildObj.transform.position,buildObj.transform.rotation);
                    if (--ItemInfoManager.instance.itemInventory[currentIndex].count <= 0)
                        AttachItem = null;

                    ItemInfoManager.instance.RefreshQuickSlots();
                    DestroyAttchdeObject();
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
        foodCoolTime += Time.deltaTime;
        if (foodCoolTime > _value)
        {
            Debug.Log("Event");
            UseFoodItem();

            if (--ItemInfoManager.instance.itemInventory[currentIndex].count <= 0)
                AttachItem = null;
            
            ItemInfoManager.instance.RefreshQuickSlots();
            DestroyAttchdeObject();
            foodCoolTime = 0;
        }
    }
    IEnumerator Attack_Coroutione()
    {
        isAttackDiley = false;
        Debug.Log(1);
        BoxCollider.enabled = true;
        yield return new WaitForFixedUpdate();
        BoxCollider.enabled = false;
        yield return new WaitForSeconds(SurvivalGauge.Instance.AttackSpeed);
        isAttackDiley = true;
    }
    public void AttachMentItem(int _itemIndex)
    {
        AttachItem = null;
        currentIndex = 0;
        attachmentDamage = 0;
        DestroyAttchdeObject();
        if (ItemInfoManager.instance.itemInventory[_itemIndex].item != null)
        { 
            AttachItem = ItemInfoManager.instance.itemInventory[_itemIndex];
            attachmentDamage = ItemInfoManager.instance.itemInventory[_itemIndex].item.damage;
            currentIndex = _itemIndex;
            if (AttachItem.item.category != "build")
            { 
                GameObject obj = Instantiate(GetItemPrefab(ItemInfoManager.instance.itemInventory[_itemIndex].item.id.ToString()), AttachPosition.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.layer = 2;

           
            }
        }
        else
            Debug.Log("isNull");
    }
    private void UseFoodItem()
    {
        SurvivalGauge.Instance.PlayerHealth += ItemInfoManager.instance.itemInventory[currentIndex].item.value[0];
        SurvivalGauge.Instance.PlayerHunger += ItemInfoManager.instance.itemInventory[currentIndex].item.value[1];
        SurvivalGauge.Instance.Stamina += ItemInfoManager.instance.itemInventory[currentIndex].item.value[2];
    }
    private void DestroyAttchdeObject()
    {
        if (AttachPosition.transform.childCount > 0)
            Destroy(AttachPosition.transform.GetChild(0).gameObject);
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
