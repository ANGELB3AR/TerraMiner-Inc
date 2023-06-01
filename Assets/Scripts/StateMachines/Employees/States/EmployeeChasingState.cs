using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeChasingState : EmployeeBaseState
{
    public EmployeeChasingState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() { }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Fighter.GetCurrentTarget() == null)
        {
            stateMachine.SwitchState(new EmployeeIdlingState(stateMachine));
        }
        else
        {
            ChaseTarget();
        }
    }

    public override void Exit() { }

    void ChaseTarget()
    {
        if (IsWithinAttackRange())
        {
            stateMachine.SwitchState(new EmployeeFightingState(stateMachine));
        }
        else
        {
            stateMachine.Movement.MoveToPoint(stateMachine.Fighter.GetCurrentTarget().transform.position);
        }
    }
}
