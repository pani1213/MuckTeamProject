using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    public PlayerAttackAbility PlayerAttackAbility;
    public PlayerHand PlayerHand;
    public PlayerFireAbility PlayerFire;
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerHand.AttachItem != null && (PlayerHand.AttachItem.item.category == "tool" || PlayerHand.AttachItem.item.id == 1002) && other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().Hit(new DamageInfo(DamageType.Normal, (PlayerFire.Damage + PlayerHand.attachmentDamage)));

        }
        if (other.CompareTag("Boss"))
        {
            Debug.Log(00);
            other.gameObject.transform.parent.GetComponent<BossMonster>().Hit(new DamageInfo(DamageType.Normal, (PlayerFire.Damage + PlayerHand.attachmentDamage)));
        }

            if (other.CompareTag("MapResource"))
            other.GetComponent<ResourceObjScript>().Hit(new DamageInfo(DamageType.Normal, PlayerFire.Damage + PlayerHand.attachmentDamage));
    }
}
