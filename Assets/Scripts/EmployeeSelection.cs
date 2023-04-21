using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmployeeSelection : MonoBehaviour
{
    [SerializeField] List<EmployeeMovement> selectedEmployees = new List<EmployeeMovement>();
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        if (hit.transform.gameObject.TryGetComponent<EmployeeMovement>(out EmployeeMovement employee))
        {
            if (Keyboard.current.ctrlKey.isPressed)
            {
                if (selectedEmployees.Contains(employee))
                {
                    selectedEmployees.Remove(employee);
                }
                else
                {
                    selectedEmployees.Add(employee);
                }
            }
            else
            {
                selectedEmployees.Clear();
                selectedEmployees.Add(employee);
            }
        }
        else
        {
            foreach (EmployeeMovement employeeMovement in selectedEmployees)
            {
                employeeMovement.Move(hit.point);
            }
        }
    }
}
