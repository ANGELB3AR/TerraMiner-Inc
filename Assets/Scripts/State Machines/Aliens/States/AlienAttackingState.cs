using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAttackingState : AlienBaseState
{
    public AlienAttackingState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Movement.StopMoving();

        stateMachine.Animator.SetTrigger(stateMachine.attack);
    }

    public override void Tick(float deltaTime)
    {
        FaceTarget();

        if (!IsWithinAttackRange())
        {
            stateMachine.SwitchState(new AlienChasingState(stateMachine));
        }

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
