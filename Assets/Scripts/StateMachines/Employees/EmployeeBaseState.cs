using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will store methods shared between multiple Employee States
/// </summary>

public abstract class EmployeeBaseState : State
{
    protected EmployeeStateMachine stateMachine;

    public EmployeeBaseState(EmployeeStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
