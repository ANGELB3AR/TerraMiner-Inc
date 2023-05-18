using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxHandler : MonoBehaviour
{
    [SerializeField] Collider hurtBoxCollider = null;
    
    Alien alien;

    private void Awake()
    {
        alien = GetComponentInParent<Alien>();
    }

    public void ActivateHurtBox()
    {
        hurtBoxCollider.enabled = true;
    }

    public void DeactivateHurtBox()
    {
        hurtBoxCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(alien.attackDamage);
        }
    }
}
