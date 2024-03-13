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
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Monster"))
                {
                  //  hit.collider.gameObject.GetComponent<IHitable>().Hit(new DamageInfo(DamageType.Normal, 10));
                }
            }
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
                Debug.Log(0);
            }
        }
    }
    private void FixedUpdate()
    {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10, out hit);
    }
        
}
