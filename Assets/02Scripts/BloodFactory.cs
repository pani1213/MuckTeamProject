using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }

    [Header("피 효과 프리팹")]
    public GameObject BloodPrefab;

    private List<GameObject> _pool;
    public int Poolsize = 10;

    // 오브젝트 풀링 이용 
    private void Awake()
    {
        Instance = this;

        _pool = new List<GameObject>();
        for (int i = 0; i < Poolsize; i++)
        {
            GameObject bloodObject = Instantiate(BloodPrefab);
            _pool.Add(bloodObject);
            bloodObject.SetActive(false);
        }
    }

    public void Make(Vector3 position, Vector3 normal, GameObject bloodyAnimal)
    {
        foreach (GameObject bloodObject in _pool)
        {
            if (bloodObject.activeInHierarchy == false)
            {
                //bloodObject.GetComponent<DestroyTime>()?.Init();
                bloodObject.transform.position = bloodyAnimal.transform.position + Vector3.up;
                bloodObject.transform.forward = normal;
                bloodObject.SetActive(true);
                StartCoroutine(Wait1second(0.5f, bloodObject));

                break;
            }
        }
    }
    public IEnumerator Wait1second(float delay,GameObject hurtobject)
    {
        yield return new WaitForSeconds(delay);
        hurtobject.SetActive(false);

    }
    private void Update()
    {

    }

}
