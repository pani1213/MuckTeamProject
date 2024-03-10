using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRespawner : MonoBehaviour
{
    public PoolingManager poolingManager; // PoolingManager �ν��Ͻ��� �����ϱ� ���� ����
    public Transform playerTransform; // �÷��̾��� ��ġ�� �����ϱ� ���� ����
    public float minSpawnDistance = 30f; // �÷��̾�κ��� �ּ� ��ȯ �Ÿ�
    public float maxSpawnDistance = 50f; // �÷��̾�κ��� �ִ� ��ȯ �Ÿ�
    public float yOffset = 1f; // ���Ͱ� ������ ���� ������

    public Collider[] mapBoundsColliders; // ���� ��踦 ��Ÿ���� Collider �迭
    void Start()
    {
        // PoolingManager �ν��Ͻ��� ������
        poolingManager = PoolingManager.instance;

        // �÷��̾� ������Ʈ�� ã�Ƽ� ����
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // ���� �ð����� ���� ��ȯ�ϴ� �ڷ�ƾ ����
        StartCoroutine(SpawnMonstersRoutine());
    }

    IEnumerator SpawnMonstersRoutine()
    {
        // ���� �ݺ��ϸ鼭 ���� ��ȯ
        while (true)
        {
            // �÷��̾� ��ġ�� �����Ǿ� ���� ���� ���͸� ������
            if (playerTransform != null)
            {
                // �÷��̾�� ���� ������ �Ÿ��� �����ϰ� ����
                float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

                // �÷��̾� �ֺ��� ������ �������� ���͸� ��ȯ�� ��ġ�� �����
                Vector3 randomDirection = Random.insideUnitSphere;
                Vector3 spawnPosition = playerTransform.position + randomDirection.normalized * spawnDistance;

                // ���Ͱ� ������ ���� ����
                spawnPosition.y = yOffset;

                


                // �����ϰ� ���� Ÿ�� ����
                MonsterType monsterType = (MonsterType)Random.Range(0, 2);
                Debug.Log(monsterType);

                // Make �޼��带 ȣ���Ͽ� ���� ����
                poolingManager.Make(monsterType, spawnPosition);


            }

            // ������ ������ �Ŀ� ���� ���� ��ȯ
            yield return new WaitForSeconds(poolingManager.regenDelay);
        }
    }
    // ���� ��� ���� ��ġ�ϴ��� Ȯ���ϴ� �Լ�
    private bool IsInsideMapBounds(Vector3 position)
    {
        foreach (Collider collider in mapBoundsColliders)
        {
            if (!collider.bounds.Contains(position))
            {
                return false; // �ϳ��� ��� �ۿ� ��ġ�ϸ� false ��ȯ
            }
        }
        return true; // ��� Collider�� ��� ���� ��ġ�ϸ� true ��ȯ
    }

}
