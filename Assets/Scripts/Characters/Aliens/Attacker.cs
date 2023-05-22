using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Tooltip("Amount of damage dealt by attack")]
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackDistance = 0.5f;

    GameObject currentTarget = null;

    public void SetCurrentTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }

    void AttemptToAttackTarget()
    {
        if (currentTarget == null) { return; }
        if (!currentTarget.TryGetComponent<Health>(out Health targetHealth)) { return; }
        if (Vector3.Distance(transform.position, currentTarget.transform.position) !<= attackDistance) { return; }

        targetHealth.DealDamage(attackDamage);
    }
}
