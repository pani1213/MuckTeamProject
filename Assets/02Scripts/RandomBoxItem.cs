using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RandomItemType
{
    YummyNuts,  // + ���¹̳�
    Dumbell,    // + �⺻ �� 10% ����
    Broccoli,   // + �ǰ� ���
    Jetpack,    // + ���� ���̰� ����
    Dracula,    // + �ִ� ü���� óġ�� ������ �� �Ŀ����� �縸ŭ ���� (�ִ�40 ����)
    Hammer      // + ���� Ȯ�� �� ���� ���ذ� ����
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
                    // Todo: ���¹̳��� Ű����
                    SurvivalGauge survivalGauge = GameObject.FindWithTag("Player").GetComponent<SurvivalGauge>();
                    survivalGauge.Stamina = survivalGauge.MaxStamina;
                    survivalGauge.RefreshAnimation();
                    break;
                }*/
        }

        return true;
    }

}
