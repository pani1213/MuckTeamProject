using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEvent : MonoBehaviour
{
    private PlayerFireAbility _owner;

    private void Start()
    {
        _owner = GetComponentInParent<PlayerFireAbility>();
    }

    public void AttackEvent()
    {
        //Debug.Log("어택이벤트 발생!");
        _owner.AttackIfInRange();
    }
}
