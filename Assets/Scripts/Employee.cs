using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] Outline selectionOutline = null;
    [SerializeField] Outline hoverOutline = null;

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

    private void OnMouseEnter()
    {
        if (selectionOutline.enabled) { return; }

        hoverOutline.enabled = true;
    }

    private void OnMouseExit()
    {
        hoverOutline.enabled = false;
    }

    private void EmployeeSelection_OnEmployeeSelected(EmployeeMovement employee)
    {
        if (employee != this) { return; }

        selectionOutline.enabled = true;
    }

    private void EmployeeSelection_OnEmployeeDeselected(EmployeeMovement employee)
    {
        if (employee != this) { return; }

        selectionOutline.enabled = false;
    }
}
