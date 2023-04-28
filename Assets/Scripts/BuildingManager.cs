using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public void PlaceBuilding(Building building, Transform buildLocation)
    {
        Instantiate(building, buildLocation.position, Quaternion.identity);
    }
}
