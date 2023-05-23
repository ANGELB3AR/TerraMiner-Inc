using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Fighter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator = null;
    [Header("Weapon")]
    [SerializeField] WeaponSO currentWeapon = null;
    [SerializeField] Transform weaponSlot = null;
    [Header("Aiming")]
    [SerializeField] Rig aimRig = null;
    [SerializeField] Transform aimTarget = null;
    [SerializeField] Vector3 aimOffset = new Vector3();

    bool isFiring = false;

    GameObject weapon = null;
    RaycastWeapon weaponPrefab = null;
    Alien currentTarget = null;

    readonly int fire = Animator.StringToHash("Fire");

    private void Start()
    {
        EquipWeapon(currentWeapon);
    }

    private void Update()
    {
        if (!isFiring) { return; }
        if (currentTarget == null) { return; }

        aimTarget.transform.position = currentTarget.transform.position + aimOffset;
        
        if (weaponPrefab.Fire())
        {
            animator.SetTrigger(fire);
        }
        else
        {
            animator.ResetTrigger(fire);
        }
    }

    void EquipWeapon(WeaponSO newWeapon)
    {
        currentWeapon = newWeapon;
        weapon = Instantiate(currentWeapon.weaponPrefab.gameObject, weaponSlot);
        weaponPrefab = weapon.GetComponent<RaycastWeapon>();

        weaponPrefab.Initialize(currentWeapon);
    }

    void UnequipWeapon()
    {
        currentWeapon = null;
    }

    public void FireWeapon(bool status)
    {
        isFiring = status;
    }

    public void SetCurrentTarget(Alien target)
    {
        currentTarget = target;
        aimOffset = new Vector3(0f, target.GetComponent<NavMeshAgent>().height / 2, 0f);
    }

    public Alien GetCurrentTarget()
    {
        return currentTarget;
    }

    public void SetAimRigWeight(float weight)
    {
        aimRig.weight = weight;
    }
}
