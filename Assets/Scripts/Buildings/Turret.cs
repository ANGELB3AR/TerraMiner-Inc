using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    [Header("Turret Settings")]
    [SerializeField] RaycastWeapon raycastWeapon = null;
    [SerializeField] WeaponSO weaponSO = null;
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float rotationSpeed = 5f;

    GameObject currentTarget = null;

    private void Start()
    {
        raycastWeapon.Initialize(weaponSO);
    }

    private void Update()
    {
        CheckForTargets();
        
        if (currentTarget == null) { return; }

        LookAtTarget();
        raycastWeapon.Fire();
    }

    public void CheckForTargets()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, detectionRange);

        GameObject nearestTarget = null;
        float nearestTargetDistance = float.MaxValue;

        foreach (Collider target in possibleTargets)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("Alien"))
            {
                if (!target.GetComponent<Health>()!.IsAlive) { continue; }

                float targetDistance = Vector3.Distance(transform.position, target.transform.position);

                if (targetDistance >= nearestTargetDistance) { continue; }

                nearestTarget = target.gameObject;
                nearestTargetDistance = targetDistance;
            }
        }

        currentTarget = nearestTarget;
    }

    private void LookAtTarget()
    {
        Vector3.RotateTowards(transform.forward, currentTarget.transform.position, rotationSpeed * Time.deltaTime, 0.0f);
    }
}
