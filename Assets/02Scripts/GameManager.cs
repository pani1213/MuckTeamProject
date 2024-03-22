using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class GameManager : Singleton<GameManager>
{
    public Action action;
    private CameraShake _cameraShake;
    public Text _noticeText;

    public CameraShake cameraShake
    {
        get 
        {
            if(_cameraShake == null)
                _cameraShake = Camera.main.GetComponent<CameraShake>();
            return _cameraShake; 
        }
    }
    private void Start()
    {
        JsonParsingManager.instance.InIt();
        ItemInfoManager.instance.InIt();
        ResourceSpawnManager.instance.InIt();
        BuildManager.instance.InIt();

        ItemInfoManager.instance.itemInventory[18] = new InvenItem() { item = JsonParsingManager.instance.ItemDic[1002], count = 1 };
        ItemInfoManager.instance.RefreshQuickSlots();
    }
}
