using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefab : MonoBehaviour
{
    [SerializeField] Transform projectileSpawnPoint = null;

    public Transform GetProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }
}
