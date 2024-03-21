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
        SoundManager.instance.PlayAudio("TreeHit");
        if (hp <= 0)
        {
            ResourceSpawnManager.instance.spawns[index].isSpawn = false;

            DropItems();        
            Destroy(gameObject);
        }
    }
    public void DropItems()
    {
        for (int i = 0; i < JsonParsingManager.instance.resourceDictionary[id].dropItemId.Length; i++) 
        {
            int dropPercent = JsonParsingManager.instance.resourceDictionary[id].dropPercentage[i];

            if (UnityEngine.Random.Range(0, 100) < dropPercent)
            {
                ItemObjectScript item = Instantiate(ItemInfoManager.instance.itemdic[JsonParsingManager.instance.resourceDictionary[id].dropItemId[i]]);
                item.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);  
                item.InIt(JsonParsingManager.instance.resourceDictionary[id].dropItemId[i], UnityEngine.Random.Range(JsonParsingManager.instance.resourceDictionary[id].dropItemCountMinRange[i],
                    JsonParsingManager.instance.resourceDictionary[id].dropItemCountMaxRange[i]),ItemType.Item);
            }
        }
    }
}
