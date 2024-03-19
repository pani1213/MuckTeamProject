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
        if (deadBoxCount >= randomBox.Length) // 모든 Box가 죽었는지 확인
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

            // 전체 그룹을 새 위치로 이동
            transform.position = selectedLocation.position;

            // 모든 RandomBox를 재활성화 및 초기화
            foreach (var box in randomBox)
            {
                box.Reinitialize(); // RandomBox에 정의된 재초기화 메서드
            }

            deadBoxCount = 0; // 끝난 RandomBox 수 초기화
        }
    }
}
