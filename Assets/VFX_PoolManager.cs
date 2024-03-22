using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VFX_PoolManager : Singleton<VFX_PoolManager>
{

    public Dictionary<string, Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    [SerializeField]
    public List<Pool> PoolContainer = new List<Pool>();

    private void Start()
    {
        for (int i = 0; i < PoolContainer.Count; i++)
        {
            poolDic.Add(PoolContainer[i].tag, new Queue<GameObject>());
            for (int j = 0; j < PoolContainer[i].size; j++)
            {
                CreateNewObject(PoolContainer[i].tag);
            }
        }
    }
    private GameObject CreateNewObject(string _tag)
    {
        GameObject obj = null;

        for (int i = 0; i < PoolContainer.Count; i++)
        {
            if (PoolContainer[i].tag == _tag)
            {
                obj = Instantiate(PoolContainer[i].prefab, transform);
                break;
            }
        }
        if (obj != null)
        {
            obj.name = _tag;
            poolDic[_tag].Enqueue(obj);
            return obj;
        }
        else
        {
            Debug.LogError("obj null");
            return null;
        }
    }
    public void ObjectInPool(GameObject _obj)
    {
        poolDic[_obj.tag].Enqueue(_obj);
    }
    public T GetPoolObject<T>(string _objName) where T : Component
    {
        if (GetPoolObject(_objName).TryGetComponent<T>(out T component))
            return component;
        else
        {
            Debug.LogError("Not Found Component");
            return null;
        }
    }
    public GameObject GetPoolObject(string _objName)
    {
        if (poolDic[_objName].Count <= 0)
            return CreateNewObject(_objName);

        return poolDic[_objName].Dequeue();
    }
}
