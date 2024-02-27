using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    private Monster _owner;

    private void Start()
    {
        _owner = GetComponentInParent<Monster>();

    }

    public void AttackEvent()
    {
        _owner.PlayerAttack();

    }
}
