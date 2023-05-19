using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] Collider hitboxCollider = null;

    Health attackee;

    public event Action OnAttackLanded;

    private void Start()
    {
        DeactivateHitbox();
    }

    // Called from Animation Event
    public void ActivateHitbox()
    {
        hitboxCollider.enabled = true;
    }

    // Called from Animation Event
    public void DeactivateHitbox()
    {
        hitboxCollider.enabled = false;
    }

    public void DealDamage(int damageAmount)
    {
        attackee.DealDamage(damageAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected");
        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            attackee = health;
            OnAttackLanded?.Invoke();
            Debug.Log("Attacked triggered on " + other.gameObject.name);
        }
    }
}
