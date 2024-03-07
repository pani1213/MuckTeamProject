using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> MonsterPrefabs;
    public Transform MonsterPoint;

    public float SpawnTime = 30f;
    public float CurrentTimer = 0f;

    public void MakeMonster()
    {
        if (MonsterPrefabs == null)
        {
            foreach (Monster monsterPrefab in MonsterPrefabs)
            {
                Monster monsterObject = Instantiate(monsterPrefab);
            }
        }
    }
}

