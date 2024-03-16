using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + Vector3.forward * Time.deltaTime;
        }
    }
}
