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
        Debug.Log("�ݺ��� ����");
        // ���� �ݺ��ϸ鼭 ���� ��ȯ
        while (true)
        {
        Debug.Log("�ݺ��� ���ư�����");
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
                //Debug.Log(monsterType);

                // Make �޼��带 ȣ���Ͽ� ���� ����
                poolingManager.Make(monsterType, spawnPosition);
            }

            // ������ ������ �Ŀ� ���� ���� ��ȯ
            yield return new WaitForSeconds(poolingManager.regenDelay);
        }
    }
    // ���Ͱ� �׾��� �� ȣ��Ǵ� �Լ�
    public void OnMonsterDeath()
    {
        // ���Ͱ� ������ ���� �ð� �Ŀ� �ٽ� ��ȯ�ǵ��� �ڷ�ƾ ȣ��
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        Debug.Log("5�� ����");
        // ���� ���͸� �ٽ� ��ȯ�ϴ� ������ ����
        yield return new WaitForSeconds(5f); // ��: 5�� �Ŀ� �ٽ� ��ȯ
        Debug.Log("5�� ��");

        // �ٽ� ��ȯ�ϱ� ���� ���͸� �����ϴ� �Լ� ȣ��
        StartCoroutine(SpawnMonstersRoutine());
    }
}

