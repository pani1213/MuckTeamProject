using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    public List<GameObject> MonsterPrefab;

    public List<GameObject> _monsterpool;
    public int PoolSize = 20;

    public void Start()
    {
        _monsterpool = new List<GameObject>();

        for (int i = 0; i < PoolSize; i++)
        {
            foreach (GameObject prefab in MonsterPrefab)
            {
                GameObject monsters = Instantiate(prefab);
                _monsterpool.Add(monsters.GetComponent<GameObject>());
                monsters.SetActive(false);
            }
        }
    }


}
