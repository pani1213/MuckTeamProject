using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class JsonParsingManager : Singleton<JsonParsingManager>
{
    public TextAsset itemTextAsset,ResourceTextAsset;
    public Items itemData;
    public ResourceList resourceData;
    public Dictionary<int, Resources> resourceDictionary = new Dictionary<int, Resources>();
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public void InIt()
    {
        itemData = JsonUtility.FromJson<Items>(itemTextAsset.text);
        foreach (Item it in itemData.ItemData) { ItemDic.Add(it.id, it); };
        resourceData = JsonUtility.FromJson<ResourceList>(ResourceTextAsset.text);
        foreach (Resources it in resourceData.ResourcesData){resourceDictionary.Add(it.id, it);}; 
    }
}
[Serializable]
public class ResourceList
{
    public List<Resources> ResourcesData;
}
[Serializable]
public class Resources
{
    public int id;
    public string name;
    public int hp;
    public int[] dropItemId;
    public int[] dropItemCountMinRange;
    public int[] dropItemCountMaxRange;
    public int[] DropPercentage;
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

