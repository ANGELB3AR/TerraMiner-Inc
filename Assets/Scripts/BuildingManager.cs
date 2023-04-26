using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public void BuildBuilding(Building[] buildings, Transform buildLocation)
    {
        foreach (Building building in buildings)
        {
            Instantiate(building, buildLocation.position, Quaternion.identity);
        }
    }
}
