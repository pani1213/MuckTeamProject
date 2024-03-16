using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSlot : Slot
{
    //리프레쉬
    public override void Refresh_SlotUI()
    {
        //Debug.Break
        if (ItemInfoManager.instance.boxDictionary[id][slotIndex].count <= 0)
        {
            ItemInfoManager.instance.boxDictionary[id][slotIndex].item = null;
            ItemInfoManager.instance.boxDictionary[id][slotIndex].count = 0;
            Empty_UI();
            return;
        }

        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(ItemInfoManager.instance.boxDictionary[id][slotIndex].item.imageFileName);
        itemCountText_UI.text = ItemInfoManager.instance.boxDictionary[id][slotIndex].count.ToString();
    }


}
