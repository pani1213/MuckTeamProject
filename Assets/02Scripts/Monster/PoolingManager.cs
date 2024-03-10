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

        Make(MonsterType.Melee);
    }
    private Monster Get(MonsterType monsterType) // 창고 뒤지기
    {

        Debug.Log(_monsterpool.Count);
        for (int i = 0; i < _monsterpool.Count; i++)
        {
        Debug.Log(_monsterpool[i].gameObject.activeSelf);
            if (!_monsterpool[i].gameObject.activeSelf && _monsterpool[i]._monsterType == monsterType)
            {

                Debug.Log(_monsterpool[i].name);
                return _monsterpool[i];
            }
        }
        //foreach (Monster monsterObject in _monsterpool)
        //{
        //    
        //    if (monsterObject.gameObject.activeSelf  == false&& monsterObject._monsterType == monsterType)
        //    {
        //    Debug.Log("return monster");
        //        return monsterObject;
        //    }
        //
        //}
            Debug.Log("null");
        return null;
    }

    public void Make(MonsterType monsterType)
    {
        Monster monster = Get(monsterType);
        GameObject obj;
        if (monster != null)
        {
            monster.transform.position = respawnPoint.position;
            monster.transform.rotation = respawnPoint.rotation;
            monster.Init();
            monster.gameObject.SetActive(true);
            Debug.Log(0);

        }
        else
        {

            if (monsterType == MonsterType.Melee)
            {
                obj = Instantiate(MonsterPrefab[0], transform);
       
            }

            else
            {
                obj = Instantiate(MonsterPrefab[1], transform);
            }
            Debug.Log(1);
            _monsterpool.Add(obj.GetComponent<Monster>());
        }
    }

    private void ResetMonster(Monster monster)
    {
        monster.transform.position = respawnPoint.position;
        monster.transform.rotation = respawnPoint.rotation;
        monster.Init();
        monster.gameObject.SetActive(true);
    }

}

