using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_BoxItem : MonoBehaviour
{
    public static UI_BoxItem instance;
    public int maxColumns = 2; // �ִ� ���� ��
    public int maxRows = 5; // �ִ� ���� ��

    // �ڽ� EŰ ������ ��, Creating item of type: 'jumpPower'�� ���� UI ����
    public GameObject backgroundImage;
    public TextMeshProUGUI SituationText;

    private float textVisibleDuration = 3f; // �ؽ�Ʈ�� ������ �ð�
    private float textVisibleTimer = 0; // Ÿ�̸�
    private bool isTextVisible = false; // �ؽ�Ʈ�� ���� ���̴��� ����

    public TextMeshProUGUI AvocadoCountText;
    public TextMeshProUGUI BananaCountText;
    public TextMeshProUGUI BreadCountText;
    public TextMeshProUGUI BroccoliCountText;
    public TextMeshProUGUI CarrotCountText;
    public TextMeshProUGUI FishCountText;
    public TextMeshProUGUI GarlicCountText;
    public TextMeshProUGUI PearCountText;
    public TextMeshProUGUI PepperCountText;
    public TextMeshProUGUI PumpkinCountText;

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

    private GridLayoutGroup gridLayoutGroup;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        backgroundImage.SetActive(false);
        SituationText.gameObject.SetActive(false);

        gridLayoutGroup = backgroundImage.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = backgroundImage.AddComponent<GridLayoutGroup>();
        }

        // GridLayoutGroup �Ӽ� ����
        gridLayoutGroup.cellSize = new Vector2(50, 50); // ������ �� ũ�� ����
        gridLayoutGroup.spacing = new Vector2(3, 3); // ������ ���� ����
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = maxColumns; // �ִ� �� �� ����


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

    void Update()
    {
        if (isTextVisible)
        {
            textVisibleTimer += Time.deltaTime;
            if (textVisibleTimer >= textVisibleDuration)
            {
                SituationText.gameObject.SetActive(false);
                isTextVisible = false;
                textVisibleTimer = 0; // Ÿ�̸� ����
            }
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
        Refresh();
    }

    // UI�� ���ΰ�ħ �ϴ� �Լ�
    public void Refresh()
    {
        // ��� ������ �̹����� ������ ����ϴ�.
        foreach (var image in itemImages.Values)
        {
            image.SetActive(false); // �ϴ� ��� ������ �̹����� ����
        }

        // ���� ������ �ִ� ��� ������ Ÿ�Կ� ���� �ݺ��Ͽ� ó��
        foreach (var itemType in itemCounts.Keys)
        {
            // �ش� ������ Ÿ�Կ� �´� �̹����� ������ Ȱ��ȭ�ϰ� ������ ������Ʈ
            if (itemImages.ContainsKey(itemType))
            {
                itemImages[itemType].SetActive(true); // ������ �̹��� Ȱ��ȭ

                // ���� ������ �̹��� ���� ������ ǥ���� TextMeshProUGUI ������Ʈ�� ã�Ƽ� ������Ʈ
                var countText = itemImages[itemType].GetComponentInChildren<TextMeshProUGUI>(true);
                if (countText != null)
                {
                    countText.text = itemCounts[itemType].ToString(); // ������ ���� ������Ʈ
                    countText.gameObject.SetActive(true); // ������ ǥ���ϴ� �ؽ�Ʈ Ȱ��ȭ
                }
            }
        }
        backgroundImage.SetActive(true);
        AdjustBackgroundSize();

    }
    public void Refresh_TextUI(int _id)
    {
        // Debug.Log(_id);
        //Debug.Log(JsonParsingManager.instance.boxItemDic[_id].descript);
        //Debug.Log(JsonParsingManager.instance.boxItemDic[_id].value);
        SituationText.text = string.Format(JsonParsingManager.instance.boxItemDic[_id].descript, JsonParsingManager.instance.boxItemDic[_id].value);
        SituationText.gameObject.SetActive(true);
        isTextVisible = true;
        textVisibleTimer = 0; // Ÿ�̸� ����
    }

   

    // backgroundImage ũ�� ���� �Լ�
    private void AdjustBackgroundSize()
    {
        // �̹����� Ȱ��ȭ��(������ ǥ�õǴ�) �������� ������ ī��Ʈ
        int activeItemCount = 0;
        foreach (var image in itemImages.Values)
        {
            if (image.activeSelf)
            {
                activeItemCount++;
            }
        }
        // �ʿ��� �� ��� �� ���
        int totalColumnsNeeded = Mathf.Min(activeItemCount, maxColumns);
        int totalRowsNeeded = Mathf.CeilToInt((float)activeItemCount / maxColumns);

        // ����� �ʺ�� ���̸� �����մϴ�.
        float width = totalColumnsNeeded * gridLayoutGroup.cellSize.x + (totalColumnsNeeded - 1) * gridLayoutGroup.spacing.x + gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float height = totalRowsNeeded * gridLayoutGroup.cellSize.y + (totalRowsNeeded - 1) * gridLayoutGroup.spacing.y + gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

        // ������ �ʺ�� ���̷� backgroundImage�� ũ�⸦ ����
        var rectTransform = backgroundImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
