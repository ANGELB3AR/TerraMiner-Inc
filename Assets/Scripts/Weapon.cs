using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Weapon", fileName = "NewWeapon")]
public class Weapon : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject weaponPrefab = null;
    public GameObject projectilePrefab = null;

    [Header("Setup")]
    public RecoilType recoilType;
    public float timeBetweenShots;
    public int roundsInClip;
    public float timeBetweenClips;
    [HideInInspector] public Transform projectileSpawnPoint;
    
    public enum RecoilType
    {
        Single,
        Burst,
        Continuous
    }
}
