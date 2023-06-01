using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSabotagingState : AlienBaseState
{
    public AlienSabotagingState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Movement.StopMoving();

        stateMachine.Animator.SetTrigger(stateMachine.attack);
    }

    public override void Tick(float deltaTime)
    {
        FaceTarget();

        if (!stateMachine.Attacker.GetCurrentTarget().IsAlive)
        {
            stateMachine.SwitchState(new AlienIdlingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.ResetTrigger(stateMachine.attack);
    }
}
