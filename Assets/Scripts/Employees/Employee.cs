using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] EmployeeMovement movement = null;

    [SerializeField] Outline selectionOutline = null;
    [SerializeField] Outline hoverOutline = null;

    [SerializeField] EmployeeState currentState;  // Deserialize when finished testing
    EmployeeState previousState;

    Vector3 positionToMove;
    Building buildingToConstruct;
    bool isConstructingBuilding = false;

    [field:SerializeField] public int buildingSkill { get; private set; } = 1;

    public static event Action<Employee, Building> OnEmployeeStartedConstruction;
    public static event Action<Employee, Building> OnEmployeeStoppedConstruction;

    private void OnEnable()
    {
        EmployeeSelection.OnEmployeeSelected += EmployeeSelection_OnEmployeeSelected;
        EmployeeSelection.OnEmployeeDeselected += EmployeeSelection_OnEmployeeDeselected;
    }

    private void OnDisable()
    {
        EmployeeSelection.OnEmployeeSelected -= EmployeeSelection_OnEmployeeSelected;
        EmployeeSelection.OnEmployeeDeselected -= EmployeeSelection_OnEmployeeDeselected;
    }

    private void Start()
    {
        currentState = EmployeeState.Idling;
        InitializeState();
    }

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

    #region Public Methods

    public void ConstructBuilding(Building building, Vector3 location)
    {
        Debug.Log(building);
        buildingToConstruct = building;
        positionToMove = location;

        SwitchState(EmployeeState.Building);
    }

    #endregion

    #region State Machine

    private void Update()
    {
        ProcessState();
    }

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
                break;
            case EmployeeState.Building:
                movement.Move(positionToMove);
                break;
            case EmployeeState.Transporting:
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
                break;
            case EmployeeState.Fighting:
                break;
            case EmployeeState.Building:
                //Debug.Log(buildingToConstruct.transform.position);
                //Debug.Log(Vector3.Distance(gameObject.transform.position, buildingToConstruct.transform.position));
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
                break;
            case EmployeeState.Building:
                OnEmployeeStoppedConstruction?.Invoke(this, buildingToConstruct);
                isConstructingBuilding = false;
                movement.StopMoving();
                break;
            case EmployeeState.Transporting:
                break;
            default:
                break;
        }
    }


    public enum EmployeeState
    {
        Idling,
        Fighting,
        Building,
        Transporting
    }

    #endregion
}
