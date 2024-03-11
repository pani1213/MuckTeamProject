using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObjScript : MonoBehaviour, IHitable
{
    public int id;
    public int hp;
    public int index;
    public void InIt(int _index)
    {
        //transform.localPosition = Vector3.zero;
        index = _index;
        hp = JsonParsingManager.instance.resourceDictionary[id].hp;
    }
    public void Hit(DamageInfo damageInfo)
    {
        hp -= damageInfo.Amount;
        Debug.Log(hp);
        if (hp <= 0)
        {
            Debug.Log($"drop : {JsonParsingManager.instance.resourceDictionary[id].dropItemId.Length}");
            ResourceSpawnManager.instance.spawns[index].isSpawn = false;
            Destroy(gameObject);
        }
    }
}
