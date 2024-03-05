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

        Debug.Log(itemTextAsset.text);
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
    public string value;
    public List<int> value_List;
    public string imageFileName;
    public string makeResource;
    public List<int> makeResource_List;
    public string makeResourceCount;
    public List<int> makeResourceCount_List;
    public int makeCount;
    public int cookingTime;

    public List<int> StringToList(string _value)
    {
        List<int> ints = new List<int>();
        int temp = 0;
        for (int i = 0; i < _value.Length; i++)
        {
            if (_value[i] == '[' && _value[i] == ']')
                continue;

        }
    }
}


