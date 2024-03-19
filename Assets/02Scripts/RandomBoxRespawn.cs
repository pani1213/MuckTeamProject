using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxRespawn : MonoBehaviour
{
    public Transform[] respawnLocations;
    private int deadBoxCount = 0;
    private RandomBox[] randomBox;

    public static RandomBoxRespawn instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        randomBox = GetComponentsInChildren<RandomBox>();
    }

    public void OnBoxDead()
    {
        deadBoxCount++;
        if (deadBoxCount >= randomBox.Length) // ��� Box�� �׾����� Ȯ��
        {
            StartCoroutine(RespawnBoxAfterDelay(10));
        }
    }
    public IEnumerator RespawnBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (respawnLocations.Length > 0)
        {
            int index = Random.Range(0, respawnLocations.Length);
            Transform selectedLocation = respawnLocations[index];

            // ��ü �׷��� �� ��ġ�� �̵�
            transform.position = selectedLocation.position;

            // ��� RandomBox�� ��Ȱ��ȭ �� �ʱ�ȭ
            foreach (var box in randomBox)
            {
                box.Reinitialize(); // RandomBox�� ���ǵ� ���ʱ�ȭ �޼���
            }

            deadBoxCount = 0; // ���� RandomBox �� �ʱ�ȭ
        }
    }
}
