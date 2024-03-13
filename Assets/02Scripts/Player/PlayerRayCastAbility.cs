using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static UnityEditor.PlayerSettings;

public class PlayerRayCast : MonoBehaviour
{
    public Text informationText_UI;
    public PlayerHand PlayerHand;
    RaycastHit hit;
    private void Update()
    {
        if (hit.collider == null)
        {
           // informationText_UI.text = "";
        }
        else
        {
           //if (hit.collider.CompareTag("Item"))
           //    informationText_UI.text = "Prees 'E' key Get Item";
           //else
           //    informationText_UI.text = "";
           //
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
            if (PlayerHand.AttachItem != null && PlayerHand.AttachItem.item.category == "build")
            {
                BuildManager.instance.GetObject(PlayerHand.AttachItem.item.id);
                BuildManager.instance.GetObject(PlayerHand.AttachItem.item.id).transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
                BuildManager.instance.GetObject(PlayerHand.AttachItem.item.id).transform.LookAt(transform);

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject gameobj = Instantiate(ItemInfoManager.instance.itemdic[PlayerHand.AttachItem.item.id].gameObject);
                    gameobj.transform.SetPositionAndRotation(BuildManager.instance.GetObject(PlayerHand.AttachItem.item.id).transform.position,
                        BuildManager.instance.GetObject(PlayerHand.AttachItem.item.id).transform.rotation);

                    if (--ItemInfoManager.instance.itemInventory[PlayerHand.currentIndex].count <= 0)
                        PlayerHand.AttachItem = null;

                    ItemInfoManager.instance.RefreshQuickSlots();
                    if (PlayerHand.AttachPosition.transform.childCount > 0)
                        Destroy(PlayerHand.AttachPosition.transform.GetChild(0).gameObject);
                    // 상자 생성, 위치 hit.pointer
                }
            }
            if (PlayerHand.AttachItem == null || PlayerHand.AttachItem.item.category != "build")
                BuildManager.instance.ReturnObject();
            
        }
    }
    private void FixedUpdate()
    {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10, out hit);
    }
        
}
