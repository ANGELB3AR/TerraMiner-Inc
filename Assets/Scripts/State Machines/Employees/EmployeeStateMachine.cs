using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeStateMachine : StateMachine
{
    private void Start()
    {
        SwitchState(new EmployeeIdleState(this));
    }
}
