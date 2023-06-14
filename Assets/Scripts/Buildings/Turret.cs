using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] RaycastWeapon raycastWeapon = null;
    [SerializeField] WeaponSO weaponSO = null;
    [SerializeField] Building building = null;
    [SerializeField] GameObject healthBarVisual = null;
    [SerializeField] GameObject turretPivot = null;

    [SerializeField] float detectionRange = 5f;
    [SerializeField] float rotationSpeed = 5f;

    GameObject currentTarget = null;
    bool turretReady = false;

    private void OnEnable()
    {
        building.OnConstructionComplete += Building_OnConstructionComplete;
    }

    private void OnDisable()
    {
        building.OnConstructionComplete -= Building_OnConstructionComplete;
    }

    private void Start()
    {
        healthBarVisual.SetActive(false);
        raycastWeapon.enabled = false;
    }

    private void Update()
    {
        if (!turretReady) { return; }

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
        Vector3 direction = (currentTarget.transform.position - turretPivot.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        turretPivot.transform.rotation = Quaternion.Slerp(turretPivot.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Building_OnConstructionComplete()
    {
        healthBarVisual.SetActive(true);
        raycastWeapon.enabled = true;
        raycastWeapon.Initialize(weaponSO);
        turretReady = true;
    }
}
