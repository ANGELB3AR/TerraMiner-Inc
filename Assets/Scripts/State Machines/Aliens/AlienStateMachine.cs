using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStateMachine : StateMachine
{
    private void Start()
    {
        SwitchState(new AlienIdleState(this));
    }
}
