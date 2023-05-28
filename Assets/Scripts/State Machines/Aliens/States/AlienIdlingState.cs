using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienIdlingState : AlienBaseState
{
    float wanderWaitTime;
    float currentTime = 0f;

    public AlienIdlingState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Movement.StopMoving();

        if (CheckForTargets())
        {
            stateMachine.SwitchState(new AlienChasingState(stateMachine));
        }

        wanderWaitTime = Random.Range(
            stateMachine.MinWanderWaitTime, 
            stateMachine.MaxWanderWaitTime);
    }

    public override void Tick(float deltaTime)
    {
        currentTime += deltaTime;

        if (currentTime >= wanderWaitTime)
        {
            stateMachine.SwitchState(new AlienWanderingState(stateMachine));
        }
    }

    public override void Exit() { }
}
