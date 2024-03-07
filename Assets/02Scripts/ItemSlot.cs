using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    public int itemId = 0;
    public Sprite emptySprite;
    public Image itemImage_UI;
    public Text itemCountText_UI;

    public void ButtonAction_ItemSlotAction()
    {
        Debug.Log(slotIndex);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemInfoManager.instance.dragItem = JsonParsingManager.instance.itemDictionary[itemId];
        ItemInfoManager.instance.inventoryIdex = slotIndex;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag ȣ��");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag ȣ��");
        ItemInfoManager.instance.dragItem = null;
        ItemInfoManager.instance.inventoryIdex = 0;
    }
    public void Refresh_SlotUI()
    {
        if (ItemInfoManager.instance.itemInventory[slotIndex].count <= 0)
        {
            Debug.Log($"slot{slotIndex} �� count : {ItemInfoManager.instance.itemInventory[slotIndex].count}");
            itemImage_UI.sprite = emptySprite;
            itemCountText_UI.text = "";
            return;
        }
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(ItemInfoManager.instance.itemInventory[slotIndex].item.imageFileName);
        itemCountText_UI.text = ItemInfoManager.instance.itemInventory[slotIndex].count.ToString();
    }
}
