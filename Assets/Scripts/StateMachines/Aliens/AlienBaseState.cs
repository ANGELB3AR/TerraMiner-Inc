using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will store methods shared between multiple Alien States
/// </summary>

public abstract class AlienBaseState : State
{
    protected AlienStateMachine stateMachine;

    public AlienBaseState(AlienStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public bool CheckForTargets()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(stateMachine.transform.position, stateMachine.AwarenessDistance);

        GameObject nearestTarget = null;
        float nearestTargetDistance = float.MaxValue;

        foreach (Collider target in possibleTargets)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("Employee"))
            {
                if (!target.GetComponent<Health>().IsAlive) { continue; }

                float targetDistance = Vector3.Distance(stateMachine.transform.position, target.transform.position);

                if (targetDistance >= nearestTargetDistance) { continue; }

                nearestTarget = target.gameObject;
                nearestTargetDistance = targetDistance;
            }
        }

        if (nearestTarget != null)
        {
            stateMachine.Attacker.SetCurrentTarget(nearestTarget);
            return true;
        }

        foreach (Collider target in possibleTargets)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("Building"))
            {
                if (!target.GetComponent<Health>().IsAlive) { continue; }

                float targetDistance = Vector3.Distance(stateMachine.transform.position, target.transform.position);

                if (targetDistance >= nearestTargetDistance) { continue; }

                nearestTarget = target.gameObject;
                nearestTargetDistance = targetDistance;
            }
        }

        if (nearestTarget != null)
        {
            stateMachine.Attacker.SetCurrentTarget(nearestTarget);
            return true;
        }

        return false;
    }

    public bool IsWithinAttackRange()
    {
        return Vector3.Distance(stateMachine.transform.position,
            stateMachine.Attacker.GetCurrentTarget().transform.position)
            <= stateMachine.Attacker.AttackRange;
    }

    public void FaceTarget()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition = stateMachine.Attacker.GetCurrentTarget().transform.position;

        if (targetPosition != null)
        {
            stateMachine.transform.LookAt(targetPosition);
        }
    }
}
