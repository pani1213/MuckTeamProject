using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObjectScript : MonoBehaviour
{
    public int id;

    //상자를 설치했을때
    public void InIt()
    {
        if (id == 0)
        {
            id = ItemInfoManager.instance.boxDictionary.Count + 1000;
            Debug.Log($"boxID : {id}");
            ItemInfoManager.instance.boxDictionary.Add(id, new List<InvenItem>(9));

            for (int i = 0; i < 9; i++)
                ItemInfoManager.instance.boxDictionary[id].Add(new InvenItem());
        }
    }
    public void OpenBoxAction()
    {
        ItemInfoManager.instance.currentBoxId = id;
        ItemInfoManager.instance.boxGameobject.InIt();
        ItemInfoManager.instance.inventoryController.InIt(true);

    }

}
