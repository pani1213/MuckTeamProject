using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class GameManager : Singleton<GameManager>
{
    public Action action;
    private void Start()
    {
        JsonParsingManager.instance.InIt();
        ItemInfoManager.instance.InIt();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.itemData.ItemData[0]);
            action();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.itemData.ItemData[3]);
            action();
        }
    }
}
