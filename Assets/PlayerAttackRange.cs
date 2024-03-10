using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
     public PlayerAttackAbility PlayerAttackAbility;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
