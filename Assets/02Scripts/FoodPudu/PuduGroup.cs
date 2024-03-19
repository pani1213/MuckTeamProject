using UnityEngine;
using System.Collections;

public class PuduGroup : MonoBehaviour
{
    public Transform[] respawnLocations;
    private int deadPuduCount = 0;
    private FoodPudu[] pudus;

    public static PuduGroup instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pudus = GetComponentsInChildren<FoodPudu>();
    }

    public void OnPuduDead()
    {
        deadPuduCount++;
        if (deadPuduCount >= pudus.Length) // 모든 Pudu가 죽었는지 확인
        {
            StartCoroutine(RespawnPudusAfterDelay(3)); 
        }
    }
    public IEnumerator RespawnPudusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 

        if (respawnLocations.Length > 0)
        {
            int index = Random.Range(0, respawnLocations.Length);
            Transform selectedLocation = respawnLocations[index];

            // 전체 그룹을 새 위치로 이동
            transform.position = selectedLocation.position;

            // 모든 Pudu를 재활성화 및 초기화
            foreach (var pudu in pudus)
            {
                pudu.Reinitialize(); // Pudu에 정의된 재초기화 메서드
            }

            deadPuduCount = 0; // 죽은 Pudu 수 초기화
        }
    }
}
