using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Tooltip("Amount of damage dealt by attack")]
    [SerializeField] int attackDamage = 1;

    Hitbox hitbox = null;

    private void Awake()
    {
        hitbox = GetComponentInChildren<Hitbox>();
    }

    private void OnEnable()
    {
        hitbox.OnAttackLanded += Hitbox_OnAttackLanded;
    }

    private void OnDisable()
    {
        hitbox.OnAttackLanded -= Hitbox_OnAttackLanded;
    }

    private void Hitbox_OnAttackLanded()
    {
        hitbox.DealDamage(attackDamage);
    }
}
