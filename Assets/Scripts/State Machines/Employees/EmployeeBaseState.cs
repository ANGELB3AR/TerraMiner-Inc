using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EmployeeBaseState : State
{
    protected EmployeeStateMachine stateMachine;

    public EmployeeBaseState(EmployeeStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
