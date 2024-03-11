using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawnManager : MonoBehaviour
{
    public List<ResourceObjScript> resourcesObjects;
    [Serializable]
    public class SpawnResource
    {
        public Vector3 pos;
        public bool isSpawn;
        public ResourceObjScript Obj;
    }
    [SerializeField]
    public SpawnResource[] spawns;


}
