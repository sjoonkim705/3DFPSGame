using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombThrowEvent : MonoBehaviour
{
    private PlayerBombFireAbility _owner;

    private void Start()
    {
        _owner = GetComponentInParent<PlayerBombFireAbility>();

    }

    public void ThrowEvent()
    {
        _owner.BombFire();

    }
}
