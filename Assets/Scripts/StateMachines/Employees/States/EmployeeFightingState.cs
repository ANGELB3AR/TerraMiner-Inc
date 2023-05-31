using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeFightingState : EmployeeBaseState
{
    public EmployeeFightingState(EmployeeStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Fighter.OnTargetKilled += Fighter_OnTargetKilled;

        stateMachine.Movement.StopMoving();

        stateMachine.Animator.SetBool(stateMachine.isAiming, true);
        stateMachine.Fighter.SetAimRigWeights(true);
        stateMachine.Fighter.FireWeapon(true);
    }

    public override void Tick(float deltaTime)
    {
        LookAtTarget();
    }

    public override void Exit()
    {
        stateMachine.Fighter.OnTargetKilled -= Fighter_OnTargetKilled;

        stateMachine.Animator.SetBool(stateMachine.isAiming, false);
        stateMachine.Fighter.SetAimRigWeights(false);
        stateMachine.Fighter.FireWeapon(false);
    }

    private void Fighter_OnTargetKilled()
    {
        stateMachine.SwitchState(new EmployeeIdlingState(stateMachine));
    }

    private void LookAtTarget()
    {
        Vector3 lookDirection = stateMachine.Fighter.GetCurrentTarget().transform.position - stateMachine.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, lookRotation, stateMachine.LookSpeed * Time.deltaTime);
    }
}
