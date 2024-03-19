using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierManager : Singleton<BrazierManager>
{
    public Dictionary<int, List<InvenItem>> brazierDictionary = new Dictionary<int, List<InvenItem>>();
    public BrazierUIController controller;

    public int currentBrazierId = 0;

    public Action BrazierAction;

    private void Update()
    {
        if (BrazierAction != null)
            BrazierAction();
    }
    public List<InvenItem> GetBrazierDic()
    {
        return brazierDictionary[currentBrazierId];
    }
    public List<InvenItem> GetBrazierDic(int _id)
    {
        return brazierDictionary[_id];
    }
}
