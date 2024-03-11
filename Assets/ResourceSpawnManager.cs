using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawnManager : Singleton<ResourceSpawnManager>
{
    public List<ResourceObjScript> resourcesObjects;
    [Serializable]
    public class SpawnResource
    {
        public Transform pos;
        public bool isSpawn;
    }
    [SerializeField]
    public SpawnResource[] spawns;
    private float spawnTime = 0;
    public void InIt()
    {
        for (int i = 0; i < spawns.Length; i++)
        { 
            GameObject obj = Instantiate(GetRandomResourceObject(), spawns[i].pos);
            obj.GetComponent<ResourceObjScript>().InIt(i);
            spawns[i].isSpawn = true;
        }
    }
    private void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > 3)
        {
            spawnTime = 0;

            for (int i = 0; i < spawns.Length; i++) 
            {
                if (spawns[i].isSpawn)
                    continue;
                else
                {
                    if (UnityEngine.Random.Range(0, 5) == 1)
                    {
                        Instantiate(GetRandomResourceObject(), spawns[i].pos).gameObject.GetComponent<ResourceObjScript>().InIt(i);
                        spawns[i].isSpawn = true;
                    }
                }
            }
        }
    }

    public GameObject GetRandomResourceObject()
    {
        switch (UnityEngine.Random.Range(0,10)) 
        {
            case 0:case 1: case 2: case 3: case 4:
                return resourcesObjects[0].gameObject;
            case 5:case 6:case 7:
                return resourcesObjects[1].gameObject;
            case 8: case 9:
                return resourcesObjects[2].gameObject;
            default:
                return null;
        }
    }

}
