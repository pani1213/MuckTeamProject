using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SlotType
{
    None,
    Item,
    Box,
}
public class Slot : MonoBehaviour
{
    public int slotIndex;
    public InvenItem InvenItem;
    public Image itemImage_UI;
    public Text itemCountText_UI;
    public SlotType slotType_UI;

    public int id;
    public void InIt(int _id)
    {
        id = _id;
    }
    public void ButtonAction_ItemSlotAction()
    {
        Debug.Log(slotIndex);
    }
    public void Empty_UI()
    {
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite("empty");
        itemCountText_UI.text = "";
    }
    public void Refresh_SlotUI(InvenItem _invenItem)
    {
        itemImage_UI.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(_invenItem.item.imageFileName);
        itemCountText_UI.text = _invenItem.count.ToString();
    }
    public virtual void Refresh_SlotUI() 
    {
        Debug.Log(1);
    }
}
