
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    public Text titleText_UI;
    public CreateButtonSlot buttonSlot;
    public List<CreateButtonSlot> buttonSlots;
    public GameObject gridObject;
    public GameObject bubbleText;
    private int id;
    public void InIt(int _id)
    {
        id = _id;
        gameObject.SetActive(true);
        titleText_UI.text = JsonParsingManager.instance.ItemDic[id].name;
        ItemInfoManager.instance.inventoryController.InIt();
        buttonSlots = new List<CreateButtonSlot>();
        for (int i = 0; i < gridObject.transform.childCount; i++)
            Destroy(gridObject.transform.GetChild(i).gameObject);
        

        for (int i = 0; i < JsonParsingManager.instance.ItemDic[id].makeList.Length; i++)
        { 
            CreateButtonSlot slot = Instantiate(buttonSlot, gridObject.transform);
            slot.makeId = JsonParsingManager.instance.ItemDic[id].makeList[i];
            slot.gameObject.SetActive(true);
            slot.images.sprite = ItemInfoManager.instance.itemSpriteAtlas.GetSprite(JsonParsingManager.instance.ItemDic[slot.makeId].imageFileName);
            buttonSlots.Add(slot);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
        {
            bubbleText.SetActive(false);
            Exit();
        }


    }
    public void Exit()
    {
        ItemInfoManager.instance.inventoryController.InIt(false);
        gameObject.SetActive(false);
    }

}
