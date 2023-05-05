using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] Outline selectionOutline = null;
    [SerializeField] Outline hoverOutline = null;

    EmployeeState currentState;
    EmployeeState previousState;

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
