using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStateMachine : StateMachine
{
    [field: Header("Components")]
    [field: SerializeField] public Movement Movement { get; private set; } = null;
    [field: SerializeField] public Animator Animator { get; private set; } = null;
    [field: SerializeField] public Attacker Attacker { get; private set; } = null;
    [field: SerializeField] public Health Health { get; private set; } = null;

    [field: Header("Settings")]
    [field: Tooltip("Minimum time alien will idle before wandering again")]
    [field: SerializeField] public float MinWanderWaitTime { get; private set; }

    [field: Tooltip("Maximum time alien will idle before wandering again")]
    [field: SerializeField] public float MaxWanderWaitTime { get; private set; }
    
    [field: Tooltip("Minimum x- and z- coordinates alien can wander to")]
    [field: SerializeField] public Vector2 MinWanderCoordinates { get; private set; }
    
    [field: Tooltip("Maximum x- and z- coordinates alien can wander to")]
    [field: SerializeField] public Vector2 MaxWanderCoordinates { get; private set; }

    [field: Tooltip("Maximum distance alien can detect targets")]
    [field: SerializeField] public float AwarenessDistance { get; private set; }

    // Animator Hash Codes
    public readonly int attack = Animator.StringToHash("Attack1");
    public readonly int impact = Animator.StringToHash("Impact");
    public readonly int isDead = Animator.StringToHash("IsDead");


    private void OnEnable()
    {
        Health.OnDamageTaken += Health_OnDamageTaken;
        Health.OnDied += Health_OnDied;
    }

    private void OnDisable()
    {
        Health.OnDamageTaken -= Health_OnDamageTaken;
        Health.OnDied -= Health_OnDied;
    }

    private void Start()
    {
        SwitchState(new AlienIdlingState(this));
    }

    private void Health_OnDamageTaken()
    {
        SwitchState(new AlienImpactState(this));
    }

    private void Health_OnDied()
    {
        SwitchState(new AlienDyingState(this));
    }
}
