using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeChasingState : EmployeeBaseState
{
    public EmployeeChasingState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

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

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    void ChaseTarget()
    {
        if (IsWithinAttackRange())
        {
            // Switch to Fighting State
        }
        else
        {
            stateMachine.Movement.MoveToPoint(stateMachine.Fighter.GetCurrentTarget().transform.position);
        }
    }
}
