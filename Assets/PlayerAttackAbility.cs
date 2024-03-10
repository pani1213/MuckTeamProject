using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    public Animation animation;
    public BoxCollider BoxCollider;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            animation.Play("swingClip");
            StartCoroutine(Attack_Coroutione());
        }
    }
    IEnumerator Attack_Coroutione()
    {
        BoxCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        BoxCollider.enabled = false;
    } 
}
