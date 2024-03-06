using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public Sprite itemNullSprite;
    private void Start()
    {
        InIt();
        GameManager.instance.action = InIt;
    }
    public void InIt()
    {
        for (int i = 0; i < itemSlots.Length; i++) 
        {
            itemSlots[i].Refresh_SlotUI();
        }

    }
 
   

}
