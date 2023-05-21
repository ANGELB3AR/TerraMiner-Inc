using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] Transform weaponSlot = null;

    bool isFiring = false;
    bool isReloading = false;
    float currentTime = 0f;
    float timeSinceLastShotFired = 0f;
    int shotsFiredSinceLastReload = 0;


    readonly int fireSingle = Animator.StringToHash("FireSingle");
    readonly int fireBurst = Animator.StringToHash("FireBurst");
    readonly int fireContinuous = Animator.StringToHash("FireContinuous");

    private void OnDisable()
    {
        currentWeapon.OnWeaponFired -= CurrentWeapon_OnWeaponFired;
    }

    private void Update()
    {
        currentTime = Time.deltaTime;

        if (!isFiring) { return; }
        if (!AbleToFireWeapon()) { return; }

        currentWeapon.Fire();
    }

    void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentWeapon.weaponPrefab.transform.parent = weaponSlot;

        currentWeapon.projectileSpawnPoint = currentWeapon.weaponPrefab.transform.GetChild(-1);

        shotsFiredSinceLastReload = 0;

        currentWeapon.OnWeaponFired += CurrentWeapon_OnWeaponFired;
    }

    void UnequipWeapon()
    {
        currentWeapon = null;

        currentWeapon.OnWeaponFired -= CurrentWeapon_OnWeaponFired;
    }

    private void CurrentWeapon_OnWeaponFired()
    {
        timeSinceLastShotFired = currentTime;
        currentTime = 0;

        shotsFiredSinceLastReload++;
    }

    bool AbleToFireWeapon()
    {
        if (isReloading) { return false; }
        if (timeSinceLastShotFired < currentWeapon.timeBetweenShots) { return false; }
        
        if (shotsFiredSinceLastReload == currentWeapon.roundsInClip) 
        {
            ReloadWeapon();
            return false; 
        }


        return true;
    }

    private IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(currentWeapon.timeBetweenClips);
    }

    public void FireWeapon(bool status)
    {
        isFiring = status;
    }
}
