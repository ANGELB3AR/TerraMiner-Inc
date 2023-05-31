using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will store methods shared between multiple Employee States
/// </summary>

public abstract class EmployeeBaseState : State
{
    protected EmployeeStateMachine stateMachine;

    public EmployeeBaseState(EmployeeStateMachine stateMachine)
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
            if (target.gameObject.layer == LayerMask.NameToLayer("Alien"))
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
            stateMachine.Fighter.SetCurrentTarget(nearestTarget);
            return true;
        }

        return false;
    }

    public bool IsWithinAttackRange()
    {
        if (stateMachine.Fighter.GetCurrentTarget() == null) { return false; }

        return Vector3.Distance(
            stateMachine.transform.position, 
            stateMachine.Fighter.GetCurrentTarget().transform.position) 
            <= stateMachine.AttackRange;
    }
}
