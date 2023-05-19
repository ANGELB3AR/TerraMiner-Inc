using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] Collider hitboxCollider = null;

    Health attackee;

    public event Action OnAttackLanded;

    private void OnEnable()
    {
        AnimationEventHandler.OnHitboxActivated += ActivateHitbox;
        AnimationEventHandler.OnHitboxDeactivated += DeactivateHitbox;
    }

    private void OnDisable()
    {
        AnimationEventHandler.OnHitboxActivated -= ActivateHitbox;
        AnimationEventHandler.OnHitboxDeactivated -= DeactivateHitbox;
    }

    public void ActivateHitbox(Collider collider)
    {
        if (collider != hitboxCollider) { return; }

        hitboxCollider.enabled = true;
    }

    public void DeactivateHitbox(Collider collider)
    {
        if (collider != hitboxCollider) { return; }

        hitboxCollider.enabled = false;
    }

    public void DealDamage(int damageAmount)
    {
        attackee.DealDamage(damageAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            attackee = health;
            OnAttackLanded?.Invoke();
        }
    }
}
