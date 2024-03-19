using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CreateButtonSlot : MonoBehaviour
{
    public int makeId;
    List<int> indexs;
    public Image images;
    public GameObject TextBubble;
    public Text itemCountText;

    [SerializeField]
    ContentSizeFitter csf;
    public void PointerEnter()
    {
        itemCountText.text = GetItemCountText();
        TextBubble.SetActive(true);
        TextBubble.transform.position = new Vector3(gameObject.transform.position.x-150, gameObject.transform.position.y-150, gameObject.transform.position.z);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csf.transform);
    }
    public void PointerExit()
    {
        TextBubble.SetActive(false);
    }
    public void ButtonAction_CreateButtonSlot()
    {
        indexs= new List<int>();
        // 아이템 개수 다 있는지 확인, 만들기
        bool isMake = true;
        Item item = JsonParsingManager.instance.ItemDic[makeId];
        for (int i = 0; i < item.makeResource.Length; i++)
        {
            indexs.Add(ItemInfoManager.instance.GetItemIndex(JsonParsingManager.instance.ItemDic[item.makeResource[i]]));
            if (indexs[i] == -1)
            { 
                isMake = false;
                continue;
            }
            if (ItemInfoManager.instance.itemInventory[indexs[i]].count < item.makeResourceCount[i])
                isMake = false;
        }
        if (isMake)
        {
            for (int i = 0; i < indexs.Count; i++)
                ItemInfoManager.instance.itemInventory[indexs[i]].count -= item.makeResourceCount[i];
            ItemInfoManager.instance.InsertItemInventory(JsonParsingManager.instance.ItemDic[makeId], JsonParsingManager.instance.ItemDic[makeId].makeCount);

            ItemInfoManager.instance.inventoryController.RefreshAllSlot();
            Debug.Log("아이템 만듦");
        }
        else
            Debug.Log("개수부족");
    }
    private string GetItemCountText()
    {
        string temp = "";
        Item item = JsonParsingManager.instance.ItemDic[makeId];
        for (int i = 0; i < item.makeResource.Length; i++)
        {

            temp += $"{JsonParsingManager.instance.ItemDic[item.makeResource[i]].name} : {item.makeResourceCount[i]}";
            if(i+1 <= item.makeResource.Length-1)
            temp += "\n";
        }
        return temp;
    }
}
