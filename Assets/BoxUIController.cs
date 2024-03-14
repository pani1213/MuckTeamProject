using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUIController : MonoBehaviour
{
    public BoxSlot[] slots;
    public void InIt()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].InIt(ItemInfoManager.instance.currentBoxId);
            slots[i].Refresh_SlotUI();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Exit();
    }
    public void Exit()
    {
        ItemInfoManager.instance.inventoryController.InIt(false);
        gameObject.SetActive(false);
    }
}
