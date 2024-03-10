using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Animation _Animation;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _Animation.Play("OpenChest");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _Animation.Play("SpinObj");
        }

    }
}
