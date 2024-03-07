using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public InvenItem dragInven;
    public Image dragImage;
    public Text dragText;

    public void InIt_DragSlot()
    {
      
        if (dragInven != null && dragInven.item != null)
        {
            
            dragImage.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(dragInven.item.imageFileName);
            dragText.text = dragInven.count.ToString();
        }
        else
        {
            dragImage.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite("empty");
            dragText.text = "";
        }
    }
}
