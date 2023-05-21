using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] Animator animator = null;


    readonly int fireSingle = Animator.StringToHash("FireSingle");
    readonly int fireBurst = Animator.StringToHash("FireBurst");
    readonly int fireContinuous = Animator.StringToHash("FireContinuous");

    public void FireWeapon()
    {
        throw new NotImplementedException();
    }
}
