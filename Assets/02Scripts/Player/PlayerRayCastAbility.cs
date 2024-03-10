using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static UnityEditor.PlayerSettings;

public class PlayerRayCast : MonoBehaviour
{
    public Text informationText_UI;
    RaycastHit hit;
    private void Update()
    {
        if (hit.collider == null)
        {
            informationText_UI.text = "";
        }
        else
        {
            if (hit.collider.CompareTag("Item"))
                informationText_UI.text = "Prees 'E' key Get Item";
            else
                informationText_UI.text = "";

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Monster"))
                {
                    hit.collider.gameObject.GetComponent<IHitable>().Hit(new DamageInfo(DamageType.Normal, 10));
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    Debug.Log("item get");
                    hit.collider.gameObject.GetComponent<ItemObjectScript>().GetItem();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10, out hit);
    }
}
