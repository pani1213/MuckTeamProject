using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoManager : Singleton<ItemInfoManager>
{

    private const int INVENTORY_MAX_COUNT = 18;
    public List<InvenItem> itemInventory = new List<InvenItem>(INVENTORY_MAX_COUNT);

    public void InIt()
    {
        for (int i = 0; i < INVENTORY_MAX_COUNT; i++)
            itemInventory.Add(new InvenItem() { item = null, count = 0 });
    }
    public void InsertItemInventory(Item _item,int _count = 1)
    {
        int emptyIndex = GetEmptyInvenIndex();
        int itemIndex = GetItemIndex(_item);
        if (itemIndex == -1) // �ߺ��� �������� ������ -1
        {
            if (emptyIndex == -1) // ������� ������ -1
            {
                Debug.Log("������ ����");
                return;
            }
            SetInven(emptyIndex, _item, _count);
        }
        else
        {
            SetInven(itemIndex, _item, _count);
        }
    }
    /// <summary>
    /// full item return null
    /// </summary>
    /// <returns></returns>
    private int GetEmptyInvenIndex()
    {
        for (int i = 0; i < itemInventory.Count; i++)
            if (itemInventory[i].item == null)
                return i;
        return -1;
    }
    /// <summary>
    ///  do you not insert parameter? item is not vlaue
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_itemIndex"></param>
    public void SetInven(int _itemIndex , Item _item = null,int _count = 0)
    {
        itemInventory[_itemIndex].item = _item;
        itemInventory[_itemIndex].count += _count;
    }
    /// <summary>
    /// find same item Index if item category is not a "tool" 
    /// </summary>
    /// <returns></returns>
    private int GetItemIndex(Item _item)
    {
        for (int i = 0; i < itemInventory.Count; i++)
        { 
            if (_item.category != "tool" && _item == itemInventory[i].item) return i;
        }
        return -1;
    }
}