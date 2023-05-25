using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator = null;
    [SerializeField] Movement movement = null;
    [SerializeField] Fighter fighter = null;
    [SerializeField] Health health = null;
    [SerializeField] Outline selectionOutline = null;
    [SerializeField] Outline hoverOutline = null;

    [Header("Settings")]
    [Tooltip("Maximum distance employee can detect targets")]
    [SerializeField] float awarenessDistance = 5f;
    [SerializeField] float attackRange = 10f;
    [Range(0, 360)]
    [SerializeField] float lookSpeed = 15f;

    [Header("Stats")]
    [SerializeField] int constructionSkill = 1;

    EmployeeState currentState;
    EmployeeState previousState;

    Vector3 positionToMove;
    Building buildingToConstruct;
    bool isConstructingBuilding = false;
    

    readonly int isAiming = Animator.StringToHash("IsAiming");
    readonly int impact = Animator.StringToHash("Impact");
    readonly int isDead = Animator.StringToHash("IsDead");

    public enum EmployeeState
    {
        Idling,
        Fighting,
        Building,
        Transporting,
        Walking,
        Impact,
        Dying
    }

    public static event Action<Employee, Building> OnEmployeeStartedConstruction;
    public static event Action<Employee, Building> OnEmployeeStoppedConstruction;

    private void OnEnable()
    {
        EmployeeSelection.OnEmployeeSelected += EmployeeSelection_OnEmployeeSelected;
        EmployeeSelection.OnEmployeeDeselected += EmployeeSelection_OnEmployeeDeselected;

        health.OnDamageTaken += Health_OnDamageTaken;
        health.OnDied += Health_OnDied;

        fighter.OnTargetKilled += Fighter_OnTargetKilled;
    }

    private void OnDisable()
    {
        EmployeeSelection.OnEmployeeSelected -= EmployeeSelection_OnEmployeeSelected;
        EmployeeSelection.OnEmployeeDeselected -= EmployeeSelection_OnEmployeeDeselected;

        health.OnDamageTaken -= Health_OnDamageTaken;
        health.OnDied -= Health_OnDied;

        fighter.OnTargetKilled -= Fighter_OnTargetKilled;
    }

    private void Start()
    {
        currentState = EmployeeState.Idling;
        InitializeState();
    }

    private void Update()
    {
        if (!health.IsAlive) { return; }

        ProcessState();
    }

    #region State Machine

    void SwitchState(EmployeeState newState)
    {
        ExitState();

        previousState = currentState;
        currentState = newState;

        InitializeState();
    }

    void InitializeState()
    {
        switch (currentState)
        {
            case EmployeeState.Idling:
                break;
            case EmployeeState.Fighting:
                animator.SetBool(isAiming, true);
                fighter.SetAimRigWeights(true);
                CheckForTargets();
                break;
            case EmployeeState.Building:
                movement.MoveToPoint(positionToMove);
                break;
            case EmployeeState.Transporting:
                break;
            case EmployeeState.Walking:
                movement.MoveToPoint(positionToMove);
                break;
            case EmployeeState.Impact:
                animator.SetTrigger(impact);
                break;
            case EmployeeState.Dying:

                break;
            default:
                break;
        }
    }

    private void ProcessState()
    {
        switch (currentState)
        {
            case EmployeeState.Idling:
                CheckForTargets();
                break;
            case EmployeeState.Fighting:
                ChaseTarget();
                break;
            case EmployeeState.Building:
                if (Vector3.Distance(gameObject.transform.position, positionToMove) <= buildingToConstruct.ConstructingDistance && !isConstructingBuilding)
                {
                    OnEmployeeStartedConstruction?.Invoke(this, buildingToConstruct);
                    isConstructingBuilding = true;
                }

                if (buildingToConstruct.GetConstructionCompleteStatus())
                {
                    SwitchState(EmployeeState.Idling);
                }
                break;
            case EmployeeState.Transporting:
                break;
            case EmployeeState.Walking:
                if (!movement.hasReachedDestination) { return; }
                SwitchState(EmployeeState.Idling);
                break;
            case EmployeeState.Impact:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Impact")) { return; }
                if (!health.IsAlive) { return; }
                SwitchState(previousState);
                break;
            case EmployeeState.Dying:
                break;
            default:
                break;
        }
    }

    void ExitState()
    {
        switch (currentState)
        {
            case EmployeeState.Idling:
                break;
            case EmployeeState.Fighting:
                animator.SetBool(isAiming, false);
                fighter.SetAimRigWeights(false);
                fighter.FireWeapon(false);
                break;
            case EmployeeState.Building:
                OnEmployeeStoppedConstruction?.Invoke(this, buildingToConstruct);
                isConstructingBuilding = false;
                movement.StopMoving();
                break;
            case EmployeeState.Transporting:
                break;
            case EmployeeState.Walking:
                movement.StopMoving();
                break;
            case EmployeeState.Impact:
                animator.ResetTrigger(impact);
                break;
            case EmployeeState.Dying:
                break;
            default:
                break;
        }
    }


    #endregion

    #region Private Methods

    private void Health_OnDamageTaken()
    {
        SwitchState(EmployeeState.Impact);
    }

    private void Health_OnDied()
    {
        animator.SetBool(isDead, true);
        SwitchState(EmployeeState.Dying);
    }

    private void Fighter_OnTargetKilled()
    {
        SwitchState(EmployeeState.Idling);
    }

    private void CheckForTargets()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, awarenessDistance);

        foreach (Collider target in possibleTargets)
        {
            if (target.TryGetComponent<Alien>(out Alien alien))
            {
                fighter.SetCurrentTarget(alien);

                if (currentState == EmployeeState.Fighting) { return; }

                SwitchState(EmployeeState.Fighting);
                return;
            }
        }

        if (currentState != EmployeeState.Idling)
        {
            SwitchState(EmployeeState.Idling);
        }

        return;
    }

    void ChaseTarget()
    {
        if (IsWithinAttackRange())
        {
            movement.StopMoving();
            ShootAtTarget();
            return;
        }

        movement.MoveToPoint(fighter.GetCurrentTarget().transform.position);
    }

    private void ShootAtTarget()
    {
        Vector3 lookDirection = fighter.GetCurrentTarget().transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);

        fighter.FireWeapon(true);
    }

    bool IsWithinAttackRange()
    {
        if (fighter.GetCurrentTarget() == null) { return false; }

        return Vector3.Distance(transform.position, fighter.GetCurrentTarget().transform.position) <= attackRange;
    }

    #endregion

    #region Public Methods

    public int GetConstructionSkill()
    {
        return constructionSkill;
    }

    public void ConstructBuilding(Building building)
    {
        buildingToConstruct = building;
        positionToMove = building.transform.position;

        SwitchState(EmployeeState.Building);
    }

    public void MoveToPoint(Vector3 point)
    {
        positionToMove = point;

        SwitchState(EmployeeState.Walking);
    }

    #endregion

    #region Highlighting

    private void OnMouseEnter()
    {
        if (selectionOutline.enabled) { return; }

        hoverOutline.enabled = true;
    }

    private void OnMouseExit()
    {
        hoverOutline.enabled = false;
    }

    private void EmployeeSelection_OnEmployeeSelected(Employee employee)
    {
        if (employee != this) { return; }

        selectionOutline.enabled = true;
    }

    private void EmployeeSelection_OnEmployeeDeselected(Employee employee)
    {
        if (employee != this) { return; }

        selectionOutline.enabled = false;
    }

    #endregion
}
