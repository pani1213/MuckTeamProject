using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class JsonParsingManager : Singleton<JsonParsingManager>
{
    public TextAsset itemTextAsset,ResourceTextAsset,boxItemTextAsset;
    public Items itemData;
    public ResourceList resourceData;
    public BoxItems BoxItems;
    public Dictionary<int, Resources> resourceDictionary = new Dictionary<int, Resources>();
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public Dictionary<int, BoxItem> boxItemDic = new Dictionary<int, BoxItem>();
    public void InIt()
    {
        itemData = JsonUtility.FromJson<Items>(itemTextAsset.text);
        Debug.Log(itemData.ItemData[1].id);
        foreach (Item it in itemData.ItemData) { ItemDic.Add(it.id, it); };
        resourceData = JsonUtility.FromJson<ResourceList>(ResourceTextAsset.text);
        foreach (Resources it in resourceData.ResourcesData)
            resourceDictionary.Add(it.id, it);
        BoxItems = JsonUtility.FromJson<BoxItems>(boxItemTextAsset.text);
        foreach (BoxItem it in BoxItems.BoxItemData){ boxItemDic.Add(it.id, it); };
    }
}
public class BoxItems
{
    public List<BoxItem> BoxItemData;
}
[Serializable]
public class BoxItem
{
    public int id;
    public string name;
    public string type;
    public float value;
    public string imageFileName;
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
    public int[] dropPercentage;
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

