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
        ResourceSpawnManager.instance.InIt();
        BuildManager.instance.InIt();
    }
}
