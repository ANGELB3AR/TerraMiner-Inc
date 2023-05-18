using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHandler : MonoBehaviour
{
    [SerializeField] Collider hitboxCollider = null;

    int attackDamage = 1;

    public void SetAttackDamage(int damageAmount)
    {
        attackDamage = damageAmount;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(attackDamage);
        }
    }
}
