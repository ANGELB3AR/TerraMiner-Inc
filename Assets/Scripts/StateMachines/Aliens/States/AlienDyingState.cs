using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDyingState : AlienBaseState
{
    public AlienDyingState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.SetBool(stateMachine.isDead, true);

        stateMachine.Movement.StopMoving();
    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }
}
