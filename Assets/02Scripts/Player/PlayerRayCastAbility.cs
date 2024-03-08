using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerRayCast : MonoBehaviour
{
    RaycastHit hit;
    private void Update()
    {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10, out hit))
            {
                if (hit.collider.CompareTag("Monster"))
                {
                    hit.collider.gameObject.GetComponent<IHitable>().Hit(new DamageInfo(DamageType.Normal,10));
                }
            }
        }
    }
}
