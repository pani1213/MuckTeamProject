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
        if (PlayerHand.AttachItem != null && (PlayerHand.AttachItem.category == "tool" || PlayerHand.AttachItem.id == 1002) && other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>().Hit(new DamageInfo(DamageType.Normal, (PlayerFire.Damage + PlayerHand.attachmentDamage)));
        }
        if (other.CompareTag("MapResource"))
        {
            other.GetComponent<ResourceObjScript>().Hit(new DamageInfo(DamageType.Normal, PlayerFire.Damage + PlayerHand.attachmentDamage));
        }    
    }
}
