using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    public PlayerAttackAbility PlayerAttackAbility;
    public PlayerHand PlayerHand;
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerHand.AttachItem != null && (PlayerHand.AttachItem.item.category == "tool" || PlayerHand.AttachItem.item.id == 1002))
        {
            if (other.CompareTag("Monster"))
            {
                Monster monster = null;
                if (other.TryGetComponent<Monster>(out monster))
                {
                    monster.Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
                }
                else
                {
                    monster.transform.parent.GetComponent<Monster>().Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
                }
                SurvivalGauge.Instance.PlayerHealth += (int)((SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage) * SurvivalGauge.Instance.lifestealPercentage);
            }
            if (other.CompareTag("Boss"))
            {
                other.gameObject.transform.parent.GetComponent<BossMonster>().Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
                SurvivalGauge.Instance.PlayerHealth += (int)((SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage) * SurvivalGauge.Instance.lifestealPercentage);

            }
            if (other.CompareTag("Pudu"))
            {
                other.gameObject.transform.GetComponent<FoodPudu>().Hit(new DamageInfo(DamageType.Normal, (SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage)));
                SurvivalGauge.Instance.PlayerHealth += (int)((SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage) * SurvivalGauge.Instance.lifestealPercentage);

            }
            if (other.CompareTag("MapResource"))
            {
                GameObject vfx = VFX_PoolManager.instance.GetPoolObject("VFX");
                vfx.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
                vfx.SetActive(true);
                ResourceObjScript obj = other.GetComponent<ResourceObjScript>();
                if (JsonParsingManager.instance.resourceDictionary[obj.id].type == PlayerHand.AttachItem.item.type)
                    obj.Hit(new DamageInfo(DamageType.Normal, SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage));

                if ((obj.id == 1001 || obj.id == 1002) && PlayerHand.AttachItem.item.type == 3)
                    obj.Hit(new DamageInfo(DamageType.Normal, SurvivalGauge.Instance.Damage + PlayerHand.attachmentDamage));

                
            }
        }
    }
}
