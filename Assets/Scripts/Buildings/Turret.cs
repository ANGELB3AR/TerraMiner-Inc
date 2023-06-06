using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    [Header("Turret Settings")]
    [SerializeField] RaycastWeapon raycastWeapon = null;
    [SerializeField] float detectionRange = 5f;


}
