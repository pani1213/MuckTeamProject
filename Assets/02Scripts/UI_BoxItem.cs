using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BoxItem : MonoBehaviour
{
    // �ڽ� EŰ ������ ��, Creating item of type: 'jumpPower'�� ���� UI ����
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
        // ������ ���� ������Ʈ
        if (!itemCounts.ContainsKey(itemType))
        {
            Debug.Log("������ ���� ������Ʈ");
            itemCounts[itemType] = 0;
        }
        itemCounts[itemType]++;

        // UI ������Ʈ
        Refresh(itemType);
    }

    // UI�� ���ΰ�ħ �ϴ� �Լ�
    public void Refresh(BoxItemType itemType)
    {
        foreach (var image in itemImages.Values)
        {
            image.SetActive(false);
        }

        if (itemImages.ContainsKey(itemType))
        {
            // foreach �� �Ἥ �׵��� ���� ������ �� ���� �ڵ�
            itemImages[itemType].SetActive(true);
            Debug.Log("UI�� ���ΰ�ħ");
        }

        backgroundImage.SetActive(true);

            //CountText.text = $"{itemType.ToString()}: {itemCounts[itemType]}";
    }
}
