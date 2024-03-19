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
        if (deadPuduCount >= pudus.Length) // ��� Pudu�� �׾����� Ȯ��
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

            // ��ü �׷��� �� ��ġ�� �̵�
            transform.position = selectedLocation.position;

            // ��� Pudu�� ��Ȱ��ȭ �� �ʱ�ȭ
            foreach (var pudu in pudus)
            {
                pudu.Reinitialize(); // Pudu�� ���ǵ� ���ʱ�ȭ �޼���
            }

            deadPuduCount = 0; // ���� Pudu �� �ʱ�ȭ
        }
    }
}
