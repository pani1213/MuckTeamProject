using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    public Animation animation;
    public BoxCollider BoxCollider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
                animation.Play("boxOpen");

        }


    }
}
