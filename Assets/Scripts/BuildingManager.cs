using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public void PlaceBuilding(Building building, Vector3 buildLocation)
    {
        Instantiate(building, buildLocation, Quaternion.identity);
    }

    public void DestroyBuilding(Building buildingToDestroy)
    {
        Destroy(buildingToDestroy.gameObject);
    }
}
