using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeStateMachine : StateMachine
{
    [field: Header("Components")]
    [field: SerializeField] public Animator Animator { get; private set; } = null;
    [field: SerializeField] public Movement Movement { get; private set; } = null;
    [field: SerializeField] public Fighter Fighter { get; private set; } = null;
    [field: SerializeField] public Builder Builder { get; private set; } = null;
    [field: SerializeField] public Health Health { get; private set; } = null;

    [field: Header("Settings")]
    [field: SerializeField] public float AwarenessDistance { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 20f;
    [field: Range(0, 360)]
    [field: SerializeField] public float LookSpeed { get; private set; } = 200f;

    [field: Header("Stats")]
    [field: SerializeField] public int ConstructionSkill { get; private set; } = 1;

    // Animation Variables
    public readonly int isAiming = Animator.StringToHash("IsAiming");
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
        SwitchState(new EmployeeIdlingState(this));
    }

    private void Health_OnDamageTaken()
    {
        SwitchState(new EmployeeImpactState(this));
    }

    private void Health_OnDied()
    {
        SwitchState(new EmployeeDyingState(this));
    }

    #region Public Methods

    public void ConstructBuilding(Building building)
    {
        Builder.ConstructBuilding(building);
        Movement.MoveToPoint(building.transform.position);

        SwitchState(new EmployeeBuildingState(this));
    }

    #endregion
}
