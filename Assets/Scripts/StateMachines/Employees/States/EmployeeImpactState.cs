using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeImpactState : EmployeeBaseState
{
    public EmployeeImpactState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.SetTrigger(stateMachine.impact);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("Impact")) { return; }
        if (!stateMachine.Health.IsAlive) { return; }

        if (CheckForTargets())
        {
            stateMachine.SwitchState(new EmployeeChasingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new EmployeeIdlingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.ResetTrigger(stateMachine.impact);
    }
}
