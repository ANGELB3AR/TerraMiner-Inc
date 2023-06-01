using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienChasingState : AlienBaseState
{
    public AlienChasingState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() { }

    public override void Tick(float deltaTime)
    {
        ChaseTarget();

        if (IsWithinAttackRange())
        {
            if (stateMachine.Attacker.GetCurrentTarget().GetComponent<EmployeeStateMachine>())
            {
                stateMachine.SwitchState(new AlienAttackingState(stateMachine));
            }
            else if (stateMachine.Attacker.GetCurrentTarget().GetComponent<Building>())
            {
                stateMachine.SwitchState(new AlienSabotagingState(stateMachine));
            }
        }
    }

    public override void Exit() { }

    void ChaseTarget()
    {
        stateMachine.Movement.MoveToPoint(stateMachine.Attacker.GetCurrentTarget().transform.position);
    }
}
