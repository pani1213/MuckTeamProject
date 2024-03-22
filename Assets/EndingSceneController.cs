using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneController : MonoBehaviour
{
    public GameObject map1;
    public float speed;

    private void Update()
    {
        map1.transform.position += Vector3.forward * speed* Time.deltaTime;
    }
}
