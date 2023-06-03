using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeBuildingState : EmployeeBaseState
{
    public EmployeeBuildingState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() 
    {
        stateMachine.Builder.OnStartedConstruction += Builder_OnStartedConstruction;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Builder.IsConstructionComplete())
        {
            stateMachine.SwitchState(new EmployeeIdlingState(stateMachine));
        }

        if (CheckForTargets())
        {
            stateMachine.SwitchState(new EmployeeChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Builder.StopConstruction();

        stateMachine.Animator.SetBool(stateMachine.isBuilding, false);

        stateMachine.Builder.OnStartedConstruction -= Builder_OnStartedConstruction;
    }


    private void Builder_OnStartedConstruction()
    {
        stateMachine.Animator.SetBool(stateMachine.isBuilding, true);
    }

}
