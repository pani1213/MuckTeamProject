using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BoxItem : MonoBehaviour
{
    // 박스 E키 눌렀을 때, Creating item of type: 'jumpPower'에 따라 UI 생성
    public GameObject backgroundImage;
    public TextMeshPro CountText;

    private Dictionary<BoxItemType, GameObject> itemImages = new Dictionary<BoxItemType, GameObject>();
    private Dictionary<BoxItemType, int> itemCounts = new Dictionary<BoxItemType, int>();

    public GameObject AvocadoImage;
    public GameObject BananaImage;
    public GameObject BreadImage;
    public GameObject BroccoliImage;
    public GameObject CarrotImage;
    public GameObject FishImage;
    public GameObject GarlicImage;
    public GameObject PearImage;
    public GameObject PepperImage;
    public GameObject PumpkinImage;

    private void Awake()
    {
        backgroundImage.SetActive(false);

        itemImages[BoxItemType.maxhp] = AvocadoImage;
        itemImages[BoxItemType.stamina] = BreadImage;
        itemImages[BoxItemType.power] = CarrotImage;
        itemImages[BoxItemType.hp] = BroccoliImage;
        itemImages[BoxItemType.speed] = BananaImage;
        itemImages[BoxItemType.attackSpeed] = GarlicImage;
        itemImages[BoxItemType.jumpPower] = FishImage;
        itemImages[BoxItemType.hunger] = PumpkinImage;
        itemImages[BoxItemType.defense] = PepperImage;
        itemImages[BoxItemType.Lifesteal] = PearImage;

        foreach (var image in itemImages.Values)
        {
            image.SetActive(false);
        }
    }

    public void ShowBoxItem(BoxItemType itemType)
    {
        // 아이템 개수 업데이트
        if (!itemCounts.ContainsKey(itemType))
        {
            Debug.Log("아이템 개수 업데이트");
            itemCounts[itemType] = 0;
        }
        itemCounts[itemType]++;

        // UI 업데이트
        Refresh(itemType);
    }

    // UI를 새로고침 하는 함수
    public void Refresh(BoxItemType itemType)
    {
        foreach (var image in itemImages.Values)
        {
            image.SetActive(false);
        }

        if (itemImages.ContainsKey(itemType))
        {
            // foreach 문 써서 그동안 먹은 아이템 다 띄우는 코드
            itemImages[itemType].SetActive(true);
            Debug.Log("UI를 새로고침");
        }

        backgroundImage.SetActive(true);

            //CountText.text = $"{itemType.ToString()}: {itemCounts[itemType]}";
    }
}
