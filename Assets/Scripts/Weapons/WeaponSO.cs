using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Weapon", fileName = "NewWeapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject weaponPrefab = null;
    public GameObject projectilePrefab = null;

    [Header("Setup")]
    public int damageAmount;
    public float timeBetweenShots;
    public int roundsInClip;
    public float timeBetweenClips;

    [Header("Effects")]
    public ParticleSystem muzzleFlashEffect = null;
    public ParticleSystem hitEffect = null;
    public TrailRenderer tracerEffect = null;
}
