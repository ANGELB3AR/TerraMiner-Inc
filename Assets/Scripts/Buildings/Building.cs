using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] float maxBuildProgress = 100f;

    bool isBuildComplete = false;
    float currentBuildProgress = 0f;
    int totalBuildSkill = 0;
    float currentBuildSpeed = 1f;

    public bool GetBuildCompleteStatus()
    {
        return isBuildComplete;
    }

    void AddEmployeeSkill(Employee employee)
    {
        totalBuildSkill += employee.buildingSkill;
        currentBuildSpeed = 1f + (totalBuildSkill / 10f);
    }

    void RemoveEmployeeSkill(Employee employee)
    {
        totalBuildSkill -= employee.buildingSkill;
        currentBuildSpeed = 1f + (totalBuildSkill / 10f);
    }

    private void Update()
    {
        if (isBuildComplete) { return; }

        currentBuildProgress += currentBuildSpeed * Time.deltaTime;
        currentBuildProgress = Mathf.Clamp(currentBuildProgress, 0f, maxBuildProgress);

        if (currentBuildProgress != 100f) { return; }

        isBuildComplete = true;
    }
}
