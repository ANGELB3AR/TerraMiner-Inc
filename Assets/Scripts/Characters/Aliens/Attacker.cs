using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] Transform attackRaycastOrigin;
    [Tooltip("Amount of damage dealt by attack")]
    [SerializeField] int attackDamage = 1;
    [Tooltip("Maximum distance that attack can hit target")]
    public float attackRange { get; private set; } = 0.5f;

    Health currentTarget = null;
    AnimationEventHandler animationEventHandler;
    Ray ray;

    private void Awake()
    {
        animationEventHandler = GetComponentInChildren<AnimationEventHandler>();
    }

    private void OnEnable()
    {
        animationEventHandler.OnAttackAttempted += Attack;
    }

    private void OnDisable()
    {
        animationEventHandler.OnAttackAttempted -= Attack;
    }

    public void SetCurrentTarget(GameObject newTarget)
    {
        if (!newTarget.TryGetComponent<Health>(out Health targetHealth)) { return; }
        currentTarget = targetHealth;
    }

    public Health GetCurrentTarget()
    {
        return currentTarget;
    }

    void Attack()
    {
        ray.origin = attackRaycastOrigin.position;
        ray.direction = attackRaycastOrigin.forward;

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, attackRange)) { return; }
        if (!hitInfo.collider.TryGetComponent<Health>(out Health health)) { return; }

        health.DealDamage(attackDamage);

        if (currentTarget.IsAlive) { return; }

        currentTarget = null;
    }
}
