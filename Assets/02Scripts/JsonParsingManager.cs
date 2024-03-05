using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using static UnityEditor.Progress;
using UnityEngine.UI;
using JetBrains.Annotations;

public class JsonParsingManager : MonoBehaviour
{

    public TextAsset itemTextAsset;

    void Start()
    {
        Items itemData = JsonUtility.FromJson<Items>(itemTextAsset.text);
        foreach (Item it in itemData.ItemData) ;
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
    public int[] value;
    public string imageFileName;
    public int[] makeResource;
    public int[] makeResourceCount;
    public int makeCount;
    public int cookingTime;
}


