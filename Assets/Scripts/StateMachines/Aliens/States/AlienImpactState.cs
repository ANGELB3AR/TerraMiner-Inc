using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienImpactState : AlienBaseState
{
    public AlienImpactState(AlienStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.SetTrigger(stateMachine.impact);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("Impact")) { return; }

        stateMachine.SwitchState(new AlienIdlingState(stateMachine));
    }

    public override void Exit()
    {
        stateMachine.Animator.ResetTrigger(stateMachine.impact);
    }
}
