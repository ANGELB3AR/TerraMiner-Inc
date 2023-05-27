using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienWanderingState : AlienBaseState
{
    public AlienWanderingState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        WanderToNewRandomizedLocation();
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Movement.hasReachedDestination)
        {
            stateMachine.SwitchState(new AlienIdlingState(stateMachine));
        }

        if (CheckForTargetEmployees())
        {
            stateMachine.SwitchState(new AlienChasingState(stateMachine));
        }
        else if (CheckForTargetBuildings())
        {
            stateMachine.SwitchState(new AlienSabotagingState(stateMachine));
        }
    }

    public override void Exit() { }

    private void WanderToNewRandomizedLocation()
    {
        Vector3 location = new Vector3
            (Random.Range(stateMachine.MinWanderCoordinates.x, stateMachine.MaxWanderCoordinates.x),
            0f,
            Random.Range(stateMachine.MinWanderCoordinates.y, stateMachine.MaxWanderCoordinates.y));
        
        stateMachine.Movement.MoveToPoint(location);
    }
}
