using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RandomItemType
{
    YummyNuts,  // + 스태미나
    Dumbell,    // + 기본 힘 10% 증가
    Broccoli,   // + 건강 재생
    Jetpack,    // + 점프 높이가 증가
    Dracula,    // + 최대 체력은 처치할 때마다 이 파워업의 양만큼 증가 (최대40 제한)
    Hammer      // + 번개 확률 과 번개 피해가 증가
}

public class RandomBoxItem : MonoBehaviour
{
    public RandomItemType ItemType;
    public int Count;
    public RandomBoxItem(RandomItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }


    public bool TryUse()
    {
        if (Count == 0)
        {
            return false;
        }

        Count -= 1;

        switch (ItemType)
        {
            /*case ItemType.YummyNuts:
                {
                    // Todo: 스태미나를 키워줌
                    SurvivalGauge survivalGauge = GameObject.FindWithTag("Player").GetComponent<SurvivalGauge>();
                    survivalGauge.Stamina = survivalGauge.MaxStamina;
                    survivalGauge.RefreshAnimation();
                    break;
                }*/
        }

        return true;
    }

}
