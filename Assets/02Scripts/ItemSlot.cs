using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    public int slotIndex;
    public Image itemImage_UI;
    public Text itemCountText_UI;

    public void ButtonAction_ItemSlotAction()
    {
        Debug.Log(slotIndex);
    }
    public void Refresh_SlotUI()
    {
        if (ItemInfoManager.instance.itemInventory[slotIndex].count <= 0)
        {
            Empty_UI();
            return;
        }
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(ItemInfoManager.instance.itemInventory[slotIndex].item.imageFileName);
        itemCountText_UI.text = ItemInfoManager.instance.itemInventory[slotIndex].count.ToString();
    }
    public void Refresh_SlotUI(InvenItem _invenItem)
    {
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(_invenItem.item.imageFileName);
        itemCountText_UI.text = _invenItem.count.ToString();
    }
    public void Empty_UI()
    {
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite("empty");
        itemCountText_UI.text = "";
    }
}
