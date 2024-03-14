using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : Slot
{
    public override void Refresh_SlotUI()
    {
        if (ItemInfoManager.instance.itemInventory[slotIndex].count <= 0)
        {
            ItemInfoManager.instance.itemInventory[slotIndex].item = null;
            ItemInfoManager.instance.itemInventory[slotIndex].count = 0;
            Empty_UI();
            return;
        }
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(ItemInfoManager.instance.itemInventory[slotIndex].item.imageFileName);
        itemCountText_UI.text = ItemInfoManager.instance.itemInventory[slotIndex].count.ToString();
    }     
}
