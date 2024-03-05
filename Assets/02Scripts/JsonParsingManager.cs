using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class JsonParsingManager : MonoBehaviour
{

    public TextAsset itemTextAsset;

    void Start()
    {
        Items itemData = JsonUtility.FromJson<Items>(itemTextAsset.text);

        Debug.Log(itemTextAsset.text);
        foreach (Item it in itemData.ItemData)
        {
            Debug.Log(0);   
        }
        for (int i = 0; i < itemData.ItemData.Count; i++)
        {
            Debug.Log(itemData.ItemData[i].id);
        }
    }
}
[Serializable]
public class Items
{
    public List<Item> ItemData;
}
[Serializable]
public class Item
{
    public int id;
    public string name;
    public string category;
    public int damage;
    public string value;
    public string imageFileName;
    public string makeResource;
    public string makeResourceCount;
    public string makeCount;
    public int cookingTime;
}


