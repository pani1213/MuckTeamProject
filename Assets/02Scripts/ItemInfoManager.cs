using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemInfoManager : Singleton<ItemInfoManager>
{
    public InventoryController inventoryController;
    public SpriteAtlas itemSpriteAtlas;
    private const int INVENTORY_MAX_COUNT = 24;
    public const int BOX_INVENTORY_MAX_COUNT = 9;
    public List<InvenItem> itemInventory = new List<InvenItem>(INVENTORY_MAX_COUNT);
    public Dictionary<int, ItemObjectScript> itemdic = new Dictionary<int, ItemObjectScript>();
    public List<ItemObjectScript> ItemPrefabs;
    public ItemSlot[] quickSlots;

    //box
    public BoxUIController boxGameobject;
    public int currentBoxId = 0;
    public Dictionary<int, List<InvenItem>> boxDictionary = new Dictionary<int, List<InvenItem>>();

    public BrazierObject currentBrazier;

    public void InIt()
    {
        for (int i = 0; i < INVENTORY_MAX_COUNT; i++)
            itemInventory.Add(new InvenItem() { item = null, count = 0 });
        for (int i = 0; ItemPrefabs.Count > i; i++)
            itemdic.Add(ItemPrefabs[i].id, ItemPrefabs[i]);

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryController.InIt();
    }

    public void InsertItemInventory(Item _item,int _count = 1)
    { 
        int emptyIndex = GetEmptyInvenIndex();
        int itemIndex = GetItemIndex(_item);
        if (itemIndex == -1) // 중복된 아이템이 없을때 -1
        {
            if (emptyIndex == -1) // 빈공간이 없을때 -1
            {
                Debug.Log("아이템 꽉참");
                return;
            }
            SetInven(emptyIndex, _item, _count);
        }
        else
            SetInven(itemIndex, _item, _count);
    }
    public void RefreshQuickSlots()
    {
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].Refresh_SlotUI();
        }
    }
    // 인벤토리 요소 스왑 (전체스왑)
    public void InvenSwap(InvenItem invenItemA, InvenItem invenItemB)
    {

        InvenItem tempInve = new InvenItem() { item = null, count = 0};
        if (invenItemA != null)
        {
            tempInve.item = invenItemA.item;
            tempInve.count = invenItemA.count;
        }
        else
            invenItemA = new InvenItem() { item = null, count = 0 };
        if (invenItemB != null)
        {
            invenItemA.item = invenItemB.item;
            invenItemA.count = invenItemB.count;
        }
        else
            invenItemB  = new InvenItem() { item = null, count = 0 };
        invenItemB.item = tempInve.item;
        invenItemB.count = tempInve.count;

    }
    /// <summary>
    /// full item return null
    /// </summary>
    /// <returns></returns>
    private int GetEmptyInvenIndex()
    {
        for (int i = 0; i < itemInventory.Count; i++)
            if (itemInventory[i].item == null)
            {
                return i;
            }

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
    public int GetItemIndex(Item _item)
    {
        for (int i = 0; i < itemInventory.Count; i++)
        { 
            if (_item.category != "tool" && _item == itemInventory[i].item) return i;
        }
        return -1;
    }
    // 아이템 사용 스크립트 넣기
    public bool TryRemoveItem(int _id, int _count = 1)
    {
        for (int i = 0; i < itemInventory.Count; i++)
        {
            if (itemInventory[i].item == null)
                continue;

            if (itemInventory[i].item.id == _id)
            {
                if (itemInventory[i].count >= _count)
                {
                    itemInventory[i].count -= _count;
                    if (itemInventory[i].count == 0)
                        itemInventory[i].item = null;
                    return true;
                }
                else
                    Debug.Log($"item count 부족함 필요: {_count} ,소지: {itemInventory[i].count}");
            }
        }
        Debug.Log("동일한 id item 없음");
        return false;
    }
}
