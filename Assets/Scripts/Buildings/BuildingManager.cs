using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] EmployeeSelection employeeSelection = null;

    public void PlaceBuilding(Building building, Vector3 buildLocation)
    {
        Building buildingInstance = Instantiate(building, buildLocation, Quaternion.identity);

        employeeSelection.SendEmployeesToConstructBuilding(buildingInstance);
    }

    public void DestroyBuilding(Building buildingToDestroy)
    {
        Destroy(buildingToDestroy.gameObject);
    }
}
