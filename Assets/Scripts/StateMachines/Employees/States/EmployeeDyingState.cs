using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeDyingState : EmployeeBaseState
{
    public EmployeeDyingState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.SetBool(stateMachine.isDead, true);
        stateMachine.Fighter.SetAimRigWeights(false);

        stateMachine.Movement.StopMoving();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
