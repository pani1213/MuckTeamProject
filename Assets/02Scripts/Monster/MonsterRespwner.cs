using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRespawner : MonoBehaviour
{
    public PoolingManager poolingManager; // PoolingManager 인스턴스를 참조하기 위한 변수
    public Transform playerTransform; // 플레이어의 위치를 참조하기 위한 변수
    public float minSpawnDistance = 30f; // 플레이어로부터 최소 소환 거리
    public float maxSpawnDistance = 50f; // 플레이어로부터 최대 소환 거리
    public float yOffset = 1f; // 몬스터가 생성될 높이 조정값

    void Start()
    {
        // PoolingManager 인스턴스를 가져옴
        poolingManager = PoolingManager.instance;

        // 플레이어 오브젝트를 찾아서 설정
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // 일정 시간마다 몬스터 소환하는 코루틴 시작
        StartCoroutine(SpawnMonstersRoutine());
    }

    IEnumerator SpawnMonstersRoutine()
    {
        // 무한 반복하면서 몬스터 소환
        while (true)
        {
            if (!GameTimeManager.isNight)
            { 
                yield return null;
                continue;
            }
            // 플레이어 위치가 설정되어 있을 때만 몬스터를 생성함
            if (playerTransform != null)
            {
                // 플레이어 주변의 랜덤한 방향으로 몬스터를 소환할 위치를 계산함
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
                Vector3 spawnPosition = playerTransform.position + randomDirection * spawnDistance;

                // 몬스터가 생성될 높이 조정
                spawnPosition.y = yOffset;

                // 랜덤하게 몬스터 타입 선택
                MonsterType monsterType = (MonsterType)Random.Range(0, 2);

                // Make 메서드를 호출하여 몬스터 생성
                poolingManager.Make(monsterType, spawnPosition);
            }

            // 적절한 딜레이 후에 다음 몬스터 소환
            yield return new WaitForSeconds(poolingManager.regenDelay);
        }
    }

    // 몬스터가 죽었을 때 호출되는 함수
    public void OnMonsterDeath()
    {
        // 몬스터가 죽으면 일정 시간 후에 다시 소환되도록 코루틴 호출
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        // 죽은 몬스터를 다시 소환하는 딜레이 설정
        yield return new WaitForSeconds(5f); // 예: 5초 후에 다시 소환

        // 다시 소환하기 위해 몬스터를 생성하는 함수 호출
        StartCoroutine(SpawnMonstersRoutine());
    }
}