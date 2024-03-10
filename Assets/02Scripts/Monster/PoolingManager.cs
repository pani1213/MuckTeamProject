using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolingManager : Singleton<PoolingManager>
{
    public List<GameObject> MonsterPrefab;
    public List<Monster> _monsterpool;
    public int PoolSize = 20;
    public float regenDelay = 20f; // 몬스터 리젠 딜레이
    public Transform respawnPoint;
    public void Start()
    {

        _monsterpool = new List<Monster>();

        for (int i = 0; i < PoolSize; i++)
        {
            foreach (GameObject prefab in MonsterPrefab)
            {
                GameObject monsters = Instantiate(prefab);
                monsters.SetActive(false);
                _monsterpool.Add(monsters.GetComponent<Monster>());
            }
        }
    }
    private Monster Get(MonsterType monsterType) // 창고 뒤지기
    {

        
        for (int i = 0; i < _monsterpool.Count; i++)
        {
            if (!_monsterpool[i].gameObject.activeSelf && _monsterpool[i]._monsterType == monsterType)
            {
                return _monsterpool[i];
            }
        }
        return null;
    }

    public void Make(MonsterType monsterType, Vector3 position)
    {
        Monster monster = Get(monsterType);
        GameObject obj;
        if (monster != null)
        {
            monster.transform.position = position;
            monster.Init();
            monster.gameObject.SetActive(true);
           

        }
        else
        {

            if (monsterType == MonsterType.Melee)
            {
                obj = Instantiate(MonsterPrefab[0]);
                obj.transform.position = position;
            }
            else
            {
                obj = Instantiate(MonsterPrefab[1]);
                obj.transform.position = position;
            }
            _monsterpool.Add(obj.GetComponent<Monster>());
        }
    }

}

