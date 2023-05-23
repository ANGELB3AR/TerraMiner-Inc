using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Tooltip("Amount of damage dealt by attack")]
    [SerializeField] int attackDamage = 1;
    [Tooltip("Maximum distance that attack can hit target")]
    [SerializeField] float attackDistance = 0.5f;

    GameObject currentTarget = null;
    AnimationEventHandler animationEventHandler;

    private void Awake()
    {
        animationEventHandler = GetComponentInChildren<AnimationEventHandler>();
    }

    private void OnEnable()
    {
        animationEventHandler.OnAttackAttempted += AttemptToAttackTarget;
    }

    private void OnDisable()
    {
        animationEventHandler.OnAttackAttempted -= AttemptToAttackTarget;
    }

    public void SetCurrentTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }

    void AttemptToAttackTarget()
    {
        if (currentTarget == null) { return; }
        if (!currentTarget.TryGetComponent<Health>(out Health targetHealth)) { return; }
        
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= attackDistance)
        {
            targetHealth.DealDamage(attackDamage);
        }
    }
}
