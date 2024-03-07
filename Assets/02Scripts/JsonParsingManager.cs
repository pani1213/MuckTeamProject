using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class JsonParsingManager : Singleton<JsonParsingManager>
{
    public TextAsset itemTextAsset;
    public Items itemData;
    public Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();
    public void InIt()
    {
        itemData = JsonUtility.FromJson<Items>(itemTextAsset.text);
        foreach (Item it in itemData.ItemData)  {  itemDictionary.Add(it.id, it); };
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
public class InvenItem
{ 
    public Item item;
    public int count;
}

