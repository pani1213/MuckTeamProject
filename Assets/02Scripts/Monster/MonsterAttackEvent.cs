using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    private BossMonster _owner;


    private void Start()
    {
        _owner = GetComponentInParent<BossMonster>();

    }

    public void AttackEvent()
    {
        _owner.Attack();

    }
}
