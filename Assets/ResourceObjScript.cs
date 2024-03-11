using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObjScript : MonoBehaviour, IHitable
{
    public int id;
    public int hp;
    public void InIt()
    {
        hp = JsonParsingManager.instance.resourceDictionary[id].hp;
    }
    public void Hit(DamageInfo damageInfo)
    {
        hp -= damageInfo.Amount;
        Debug.Log(hp);
        if (hp <= 0)
        {
            Debug.Log($"drop : {JsonParsingManager.instance.resourceDictionary[id].dropItemId[0]}");
            gameObject.SetActive(false);
        }
    }
}
