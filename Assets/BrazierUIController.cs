using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BrazierUIController : MonoBehaviour
{
    public int id;
    public Slot fuelSlot, resourceSlot, finishedSlot;
    public Image fill_ImageUI, resourceImageUI;
    public void InIt(int _id, float _a, float _b)
    {
        id = _id;
        gameObject.SetActive(true);

        RefreshAllSlot();
        fill_ImageUI.fillAmount = _a;
        resourceImageUI.fillAmount = _b;
    }
    public void RefreshAllSlot()
    {
        fuelSlot.Refresh_SlotUI();
        resourceSlot.Refresh_SlotUI();
        finishedSlot.Refresh_SlotUI();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            ItemInfoManager.instance.inventoryController.InIt(false);
        }
    }
}
