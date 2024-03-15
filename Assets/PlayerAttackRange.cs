using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    public PlayerAttackAbility PlayerAttackAbility;
    public PlayerHand PlayerHand;
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerHand.AttachItem != null && (PlayerHand.AttachItem.item.category == "tool" || PlayerHand.AttachItem.item.id == 1002) && other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
            SurvivalGauge.Instance.PlayerHealth += (int)((SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage) * SurvivalGauge.Instance.lifestealPercentage);
        }
        if (other.CompareTag("Boss"))
        {
            Debug.Log(00);
            other.gameObject.transform.parent.GetComponent<BossMonster>().Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
            SurvivalGauge.Instance.PlayerHealth += (int)((SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage) * SurvivalGauge.Instance.lifestealPercentage);

        }
        if (other.CompareTag("Pudu"))
        {
            Debug.Log("Pudu");
            other.gameObject.transform.GetComponent<FoodPudu>().Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
            SurvivalGauge.Instance.PlayerHealth += (int)((SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage) * SurvivalGauge.Instance.lifestealPercentage);

        }
        if (other.CompareTag("MapResource"))
            other.GetComponent<ResourceObjScript>().Hit(new DamageInfo(DamageType.Normal, SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage));
    }
}
