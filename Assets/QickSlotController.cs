using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QickSlotController : MonoBehaviour
{
    public ItemSlot[] slots;
    private void Start()
    {
        InIt();
    }
    public void Update()
    {
        if (Input.GetKeyDown((KeyCode)49))
        {
            Debug.Log(1);
        }
        if (Input.GetKeyDown((KeyCode)50))
        {
            Debug.Log(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {

        }

    }


    public void InIt()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Refresh_SlotUI();
    }




}
