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

    public bool CheckForTargetEmployees()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(stateMachine.transform.position, stateMachine.AwarenessDistance);

        foreach (Collider target in possibleTargets)
        {
            if (target.TryGetComponent<Employee>(out Employee employee))
            {
                stateMachine.Attacker.SetCurrentTarget(employee.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool CheckForTargetBuildings()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(stateMachine.transform.position, stateMachine.AwarenessDistance);

        foreach (Collider target in possibleTargets)
        {
            if (target.TryGetComponent<Building>(out Building building))
            {
                stateMachine.Attacker.SetCurrentTarget(building.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool IsWithinAttackRange()
    {
        return Vector3.Distance(stateMachine.transform.position,
            stateMachine.Attacker.GetCurrentTarget().transform.position)
            <= stateMachine.Attacker.attackRange;
    }

    public void FaceTarget()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition = stateMachine.Attacker.GetCurrentTarget().transform.position;

        stateMachine.transform.LookAt(targetPosition);
    }
}
