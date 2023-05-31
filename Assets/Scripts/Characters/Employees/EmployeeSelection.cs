using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmployeeSelection : MonoBehaviour
{
    List<EmployeeStateMachine> selectedEmployees = new List<EmployeeStateMachine>();
    Camera mainCamera;

    public static event Action<EmployeeStateMachine> OnEmployeeSelected;
    public static event Action<EmployeeStateMachine> OnEmployeeDeselected;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputReader.LeftClickHitEmployee += InputReader_LeftClickHitEmployee;
        InputReader.LeftClickHitTerrain += InputReader_LeftClickHitTerrain;
        InputReader.LeftClickHitBuilding += InputReader_LeftClickHitBuilding;
        InputReader.LeftClickHitNothing += InputReader_LeftClickHitNothing;
    }

    private void OnDisable()
    {
        InputReader.LeftClickHitEmployee -= InputReader_LeftClickHitEmployee;
        InputReader.LeftClickHitTerrain -= InputReader_LeftClickHitTerrain;
        InputReader.LeftClickHitBuilding -= InputReader_LeftClickHitBuilding;
        InputReader.LeftClickHitNothing -= InputReader_LeftClickHitNothing;
    }

    private void InputReader_LeftClickHitEmployee(EmployeeStateMachine employee)
    {
        if (Keyboard.current.ctrlKey.isPressed)
        {
            if (selectedEmployees.Contains(employee))
            {
                selectedEmployees.Remove(employee);
                OnEmployeeDeselected?.Invoke(employee);
            }
            else
            {
                SelectEmployee(employee);
            }
        }
        else
        {
            ClearSelectedEmployees();
            SelectEmployee(employee);
        }
    }

    private void InputReader_LeftClickHitTerrain(Vector3 position)
    {
        MoveSelectedEmployees(position);
    }

    private void InputReader_LeftClickHitBuilding(Building building)
    {
        if (!building.GetConstructionCompleteStatus())
        {
            SendEmployeesToConstructBuilding(building);
        }

        // Add logic for when building construction is already complete
    }

    private void InputReader_LeftClickHitNothing()
    {
        ClearSelectedEmployees();
    }

    private void SelectEmployee(EmployeeStateMachine employee)
    {
        selectedEmployees.Add(employee);
        OnEmployeeSelected?.Invoke(employee);
    }

    void ClearSelectedEmployees()
    {
        foreach (EmployeeStateMachine employee in selectedEmployees)
        {
            OnEmployeeDeselected?.Invoke(employee);
        }
        selectedEmployees.Clear();
    }
    void MoveSelectedEmployees(Vector3 positionToMove)
    {
        foreach (EmployeeStateMachine employee in selectedEmployees)
        {
            employee.Movement.MoveToPoint(positionToMove);
        }
    }

    public void SendEmployeesToConstructBuilding(Building building)
    {
        foreach (EmployeeStateMachine employee in selectedEmployees)
        {
            employee.ConstructBuilding(building);
        }
    }
}
