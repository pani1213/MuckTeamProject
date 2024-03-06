using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
