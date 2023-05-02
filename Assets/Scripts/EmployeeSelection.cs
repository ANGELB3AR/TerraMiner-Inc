using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmployeeSelection : MonoBehaviour
{
    List<EmployeeMovement> selectedEmployees = new List<EmployeeMovement>();
    Camera mainCamera;

    public static event Action<EmployeeMovement> OnEmployeeSelected;
    public static event Action<EmployeeMovement> OnEmployeeDeselected;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputReader.LeftClickHitEmployee += InputReader_LeftClickHitEmployee;
    }

    private void OnDisable()
    {
        InputReader.LeftClickHitEmployee -= InputReader_LeftClickHitEmployee;
    }

    private void InputReader_LeftClickHitEmployee(EmployeeMovement employee)
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
    }

    private void SelectEmployee(EmployeeMovement employee)
    {
        selectedEmployees.Add(employee);
        OnEmployeeSelected?.Invoke(employee);
    }

    void ClearSelectedEmployees()
    {
        foreach (EmployeeMovement employeeMovement in selectedEmployees)
        {
            OnEmployeeDeselected?.Invoke(employeeMovement);
        }
        selectedEmployees.Clear();
    }
    void MoveSelectedEmployees(RaycastHit hit)
    {
        foreach (EmployeeMovement employeeMovement in selectedEmployees)
        {
            employeeMovement.Move(hit.point);
        }
    }
}
