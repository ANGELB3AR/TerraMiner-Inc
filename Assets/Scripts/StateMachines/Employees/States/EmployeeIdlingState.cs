using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeIdlingState : EmployeeBaseState
{
    public EmployeeIdlingState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() 
    {
        stateMachine.Movement.StopMoving();
    }

    public override void Tick(float deltaTime)
    {
        if (CheckForTargets())
        {
            stateMachine.SwitchState(new EmployeeChasingState(stateMachine));
        }
    }

    public override void Exit() { }
}
