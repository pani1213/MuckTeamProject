using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolingManager : Singleton<PoolingManager>
{
    public List<GameObject> MonsterPrefab;
    public Transform respawnPoint; // ������ ��ġ ����

    private List<Monster> _monsterpool;
    public int PoolSize = 20;
    public float regenDelay = 20f; // ���� ���� ������

    public void Start()
    {
        _monsterpool = new List<Monster>();

        for (int i = 0; i < PoolSize; i++)
        {
            foreach (GameObject prefab in MonsterPrefab)
            {
                GameObject monsters = Instantiate(prefab);
                _monsterpool.Add(monsters.GetComponent<Monster>());
                monsters.SetActive(false);
            }
        }
    }
    private Monster Get(MonsterType monsterType) // â�� ������

    {
        foreach (Monster monsterObject in _monsterpool)
        {
            if (!monsterObject.gameObject.activeSelf && monsterObject._monsterType == monsterType)
            {
                return monsterObject;
            }

        }
        return null;
    }

    public void Make(MonsterType monsterType)
    {
        Monster monster = Get(monsterType);
        //GameObject obj;
        if (monster != null)
        {
            monster.transform.position = respawnPoint.position;
            monster.transform.rotation = respawnPoint.rotation;
            monster.Init();
            monster.gameObject.SetActive(true);
        }
        else
        {
            GameObject obj = Instantiate(MonsterPrefab[(int)monsterType], respawnPoint.position, respawnPoint.rotation);
            _monsterpool.Add(obj.GetComponent<Monster>());
            /*        if (monsterType == MonsterType.Melee)
                    {
                        obj = Instantiate(MonsterPrefab[0], transform);
                    }

                    else
                    {
                        obj = Instantiate(MonsterPrefab[1], transform);
                    }


                    _monsterpool.Add(obj.GetComponent<Monster>());*/
        }
    }

    public void ReturnMonster(Monster monster)
    {
        if (_monsterpool.Contains(monster))
        {
            // ���͸� ��Ȱ��ȭ�ϰ� ���� �ð� �Ŀ� �����ϵ��� ����
            StartCoroutine(RegenerateMonster(monster, regenDelay));
            monster.gameObject.SetActive(true);
        }
    }
    private IEnumerator RegenerateMonster(Monster monster, float delay)
    {
        monster.gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        Make(monster._monsterType);
    }
}

