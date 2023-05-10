using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] float maxConstructionProgress = 100f;
    [SerializeField] string buildingName = null;
    [SerializeField] Sprite buildingIcon = null;

    bool isConstructionComplete = false;
    float currentConstructionProgress = 0f;
    int totalBuildSkill = 0;
    float currentConstructionSpeed = 1f;

    [field:SerializeField] public float ConstructingDistance { get; private set; } = 2f;

    public bool GetConstructionCompleteStatus()
    {
        return isConstructionComplete;
    }

    public float GetMaxConstructionProgress()
    {
        return maxConstructionProgress;
    }

    public float GetCurrentConstructionProgress()
    {
        return currentConstructionProgress;
    }

    public string GetBuildingName()
    {
        return buildingName;
    }

    public Sprite GetBuildingIcon()
    {
        return buildingIcon;
    }

    private void OnEnable()
    {
        Employee.OnEmployeeStartedConstruction += AddEmployeeSkill;
        Employee.OnEmployeeStoppedConstruction += RemoveEmployeeSkill;
    }

    private void OnDisable()
    {
        Employee.OnEmployeeStartedConstruction -= AddEmployeeSkill;
        Employee.OnEmployeeStoppedConstruction -= RemoveEmployeeSkill;
    }

    void AddEmployeeSkill(Employee employee, Building building)
    {
        if (building != this) { return; }

        totalBuildSkill += employee.buildingSkill;
        currentConstructionSpeed = 1f + (totalBuildSkill / 10f);
    }

    void RemoveEmployeeSkill(Employee employee, Building building)
    {
        if (building != this) { return; }

        totalBuildSkill -= employee.buildingSkill;
        currentConstructionSpeed = 1f + (totalBuildSkill / 10f);
    }

    private void Update()
    {
        if (isConstructionComplete) { return; }
        if (totalBuildSkill == 0) { return; }

        currentConstructionProgress += currentConstructionSpeed * Time.deltaTime;
        currentConstructionProgress = Mathf.Clamp(currentConstructionProgress, 0f, maxConstructionProgress);

        if (currentConstructionProgress != maxConstructionProgress) { return; }

        isConstructionComplete = true;
    }
}
