using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmployeeSelection : MonoBehaviour
{
    List<Employee> selectedEmployees = new List<Employee>();
    Camera mainCamera;

    public static event Action<Employee> OnEmployeeSelected;
    public static event Action<Employee> OnEmployeeDeselected;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputReader.LeftClickHitEmployee += InputReader_LeftClickHitEmployee;
        InputReader.LeftClickHitTerrain += InputReader_LeftClickHitTerrain;
        InputReader.LeftClickHitNothing += InputReader_LeftClickHitNothing;
    }

    private void OnDisable()
    {
        InputReader.LeftClickHitEmployee -= InputReader_LeftClickHitEmployee;
        InputReader.LeftClickHitTerrain -= InputReader_LeftClickHitTerrain;
        InputReader.LeftClickHitNothing += InputReader_LeftClickHitNothing;
    }

    private void InputReader_LeftClickHitEmployee(Employee employee)
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

    private void InputReader_LeftClickHitNothing()
    {
        ClearSelectedEmployees();
    }

    private void SelectEmployee(Employee employee)
    {
        selectedEmployees.Add(employee);
        OnEmployeeSelected?.Invoke(employee);
    }

    void ClearSelectedEmployees()
    {
        foreach (Employee employee in selectedEmployees)
        {
            OnEmployeeDeselected?.Invoke(employee);
        }
        selectedEmployees.Clear();
    }
    void MoveSelectedEmployees(Vector3 positionToMove)
    {
        foreach (Employee employee in selectedEmployees)
        {
            employee.MoveToPoint(positionToMove);
        }
    }

    public void SendEmployeesToConstructBuilding(Building building)
    {
        foreach (Employee employee in selectedEmployees)
        {
            employee.ConstructBuilding(building);
        }
    }
}
