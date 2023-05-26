using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will store methods shared between multiple Alien States
/// </summary>

public abstract class AlienBaseState : State
{
    protected AlienStateMachine stateMachine;

    public AlienBaseState(AlienStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
