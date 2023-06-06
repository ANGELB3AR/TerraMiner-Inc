using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    Building buildingToConstruct;
    bool isConstructingBuilding = false;
    EmployeeStateMachine employee;

    public event Action OnStartedConstruction;

    public static event Action<EmployeeStateMachine, Building> OnEmployeeStartedConstruction;
    public static event Action<EmployeeStateMachine, Building> OnEmployeeStoppedConstruction;


    private void Start()
    {
        employee = GetComponent<EmployeeStateMachine>();
    }

    private void Update()
    {
        if (buildingToConstruct == null) { return; }

        if (IsWithinConstructionRange() && isConstructingBuilding == false)
        {
            BeginConstruction();
        }

        if (IsConstructionComplete())
        {
            StopConstruction();
        }
    }

    void BeginConstruction()
    {
        employee.Movement.StopMoving();
        isConstructingBuilding = true;
        OnEmployeeStartedConstruction?.Invoke(employee, buildingToConstruct);
        OnStartedConstruction?.Invoke();
    }

    bool IsWithinConstructionRange()
    {
        return Vector3.Distance(transform.position, buildingToConstruct.transform.position) <= buildingToConstruct.ConstructingDistance;
    }

    public void ConstructBuilding(Building building)
    {
        buildingToConstruct = building;
    }

    public void StopConstruction()
    {
        isConstructingBuilding = false;
        OnEmployeeStoppedConstruction?.Invoke(employee, buildingToConstruct);
        buildingToConstruct = null;
    }

    public bool IsConstructionComplete()
    {
        return buildingToConstruct.isConstructionComplete;
    }
}
