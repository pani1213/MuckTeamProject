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
    public int maxColumns = 2; // 최대 열의 수
    public int maxRows = 5; // 최대 행의 수

    // 박스 E키 눌렀을 때, Creating item of type: 'jumpPower'에 따라 UI 생성
    public GameObject backgroundImage;
    public TextMeshProUGUI SituationText;

    private float textVisibleDuration = 3f; // 텍스트가 보여질 시간
    private float textVisibleTimer = 0; // 타이머
    private bool isTextVisible = false; // 텍스트가 현재 보이는지 여부

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

        // GridLayoutGroup 속성 설정
        gridLayoutGroup.cellSize = new Vector2(50, 50); // 적절한 셀 크기 설정
        gridLayoutGroup.spacing = new Vector2(3, 3); // 적절한 간격 설정
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = maxColumns; // 최대 열 수 설정


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
                textVisibleTimer = 0; // 타이머 리셋
            }
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
        Refresh();
    }

    // UI를 새로고침 하는 함수
    public void Refresh()
    {
        // 모든 아이템 이미지와 개수를 숨깁니다.
        foreach (var image in itemImages.Values)
        {
            image.SetActive(false); // 일단 모든 아이템 이미지를 숨김
        }

        // 현재 가지고 있는 모든 아이템 타입에 대해 반복하여 처리
        foreach (var itemType in itemCounts.Keys)
        {
            // 해당 아이템 타입에 맞는 이미지가 있으면 활성화하고 개수를 업데이트
            if (itemImages.ContainsKey(itemType))
            {
                itemImages[itemType].SetActive(true); // 아이템 이미지 활성화

                // 개별 아이템 이미지 옆에 개수를 표시할 TextMeshProUGUI 컴포넌트를 찾아서 업데이트
                var countText = itemImages[itemType].GetComponentInChildren<TextMeshProUGUI>(true);
                if (countText != null)
                {
                    countText.text = itemCounts[itemType].ToString(); // 아이템 개수 업데이트
                    countText.gameObject.SetActive(true); // 개수를 표시하는 텍스트 활성화
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
        textVisibleTimer = 0; // 타이머 시작
    }

   

    // backgroundImage 크기 조절 함수
    private void AdjustBackgroundSize()
    {
        // 이미지가 활성화된(실제로 표시되는) 아이템의 개수를 카운트
        int activeItemCount = 0;
        foreach (var image in itemImages.Values)
        {
            if (image.activeSelf)
            {
                activeItemCount++;
            }
        }
        // 필요한 총 행과 열 계산
        int totalColumnsNeeded = Mathf.Min(activeItemCount, maxColumns);
        int totalRowsNeeded = Mathf.CeilToInt((float)activeItemCount / maxColumns);

        // 배경의 너비와 높이를 조절합니다.
        float width = totalColumnsNeeded * gridLayoutGroup.cellSize.x + (totalColumnsNeeded - 1) * gridLayoutGroup.spacing.x + gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float height = totalRowsNeeded * gridLayoutGroup.cellSize.y + (totalRowsNeeded - 1) * gridLayoutGroup.spacing.y + gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

        // 조절된 너비와 높이로 backgroundImage의 크기를 설정
        var rectTransform = backgroundImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
