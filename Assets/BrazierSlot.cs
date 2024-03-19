using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierSlot : Slot
{
    public override void Refresh_SlotUI()
    {
        // brazierManager.dic[currentid] 
        // brazierManager.controller.refresh()
        if (BrazierManager.instance.GetBrazierDic()[slotIndex].count <= 0)
        {
            BrazierManager.instance.GetBrazierDic()[slotIndex].item = null;
            BrazierManager.instance.GetBrazierDic()[slotIndex].count = 0;
            Empty_UI();
            return;
        }
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(
            BrazierManager.instance.GetBrazierDic()[slotIndex].item.imageFileName);
        itemCountText_UI.text = 
            BrazierManager.instance.GetBrazierDic()[slotIndex].count.ToString();
    }
}
