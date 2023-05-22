using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Fighter : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] Transform weaponSlot = null;
    [SerializeField] Rig aimRig = null;
    [SerializeField] Transform aimTarget = null;
    [SerializeField] Vector3 aimOffset = new Vector3();

    bool isFiring = false;
    bool isReloading = false;
    float currentTime = 0f;
    float timeSinceLastShotFired = Mathf.Infinity;
    int shotsFiredSinceLastReload = 0;

    GameObject weapon = null;
    WeaponPrefab weaponPrefab = null;
    Transform projectileSpawnPoint = null;
    Alien currentTarget = null;


    readonly int fireSingle = Animator.StringToHash("FireSingle");
    readonly int fireBurst = Animator.StringToHash("FireBurst");
    readonly int fireContinuous = Animator.StringToHash("FireContinuous");

    private void Start()
    {
        EquipWeapon(currentWeapon);
    }

    private void Update()
    {
        currentTime = Time.deltaTime;

        if (!isFiring) { return; }

        aimTarget.transform.position = currentTarget.transform.position + aimOffset;

        if (!AbleToFireWeapon()) 
        {
            //animator.ResetTrigger(fireSingle);
            //animator.ResetTrigger(fireBurst);
            //animator.ResetTrigger(fireContinuous);
            return; 
        }

        Fire();
    }

    void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        
        weapon = Instantiate(currentWeapon.weaponPrefab.gameObject, weaponSlot);
        weaponPrefab = weapon.GetComponent<WeaponPrefab>();
        projectileSpawnPoint = weaponPrefab.GetProjectileSpawnPoint();

        shotsFiredSinceLastReload = 0;
    }

    void UnequipWeapon()
    {
        currentWeapon = null;
    }

    private void CurrentWeapon_OnWeaponFired()
    {
        timeSinceLastShotFired = currentTime;
        currentTime = 0;

        shotsFiredSinceLastReload++;

        HandleFiringAnimation();
    }

    private void HandleFiringAnimation()
    {
        switch (currentWeapon.recoilType)
        {
            case Weapon.RecoilType.Single:
                animator.SetTrigger(fireSingle);
                break;
            case Weapon.RecoilType.Burst:
                animator.SetTrigger(fireBurst);
                break;
            case Weapon.RecoilType.Continuous:
                animator.SetTrigger(fireContinuous);
                break;
            default:
                break;
        }
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
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.timeBetweenClips);

        isReloading = false;
        shotsFiredSinceLastReload = 0;
    }

    public void FireWeapon(bool status)
    {
        isFiring = status;
    }

    public void SetCurrentTarget(Alien target)
    {
        currentTarget = target;
    }

    public Alien GetCurrentTarget()
    {
        return currentTarget;
    }

    void Fire()
    {
        Instantiate(currentWeapon.projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        Debug.Log("WeaponFired");
    }
}
