using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
    public Text titleText_UI;
    public CreateButtonSlot buttonSlot;
    public List<CreateButtonSlot> buttonSlots;
    public GameObject gridObject;

    private int id;
    public void InIt(int _id)
    {
        gameObject.SetActive(true);
        ItemInfoManager.instance.inventoryController.InIt();
        buttonSlots = new List<CreateButtonSlot>();
        id = _id;
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
            Exit();


    }
    public void Exit()
    {
        ItemInfoManager.instance.inventoryController.InIt(false);
        gameObject.SetActive(false);
    }

}