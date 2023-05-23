using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO = null;
    [SerializeField] Transform raycastOrigin = null;

    Ray ray;
    RaycastHit hitInfo;
    bool readyToFire = true;
    bool isReloading = false;
    float bulletCooldown;
    float clipCooldown;
    int bulletsRemainingInClip;
    ParticleSystem muzzleFlashEffect;
    ParticleSystem bulletHitEffect;
    TrailRenderer tracerEffect;

    private void Update()
    {
        bulletCooldown -= Time.deltaTime;

        if (bulletsRemainingInClip == 0)
        {
            isReloading = true;
            clipCooldown -= Time.deltaTime;
        }

        if (clipCooldown <= 0f)
        {
            isReloading = false;
            clipCooldown = weaponSO.timeBetweenClips;
            bulletsRemainingInClip = weaponSO.roundsInClip;
        }

        if (bulletCooldown <= 0f && !isReloading)
        {
            readyToFire = true;
        }
    }

    public void Initialize(WeaponSO weaponSO)
    {
        this.weaponSO = weaponSO;

        bulletCooldown = weaponSO.timeBetweenShots;
        clipCooldown = weaponSO.timeBetweenClips;
        bulletsRemainingInClip = weaponSO.roundsInClip;

        muzzleFlashEffect = Instantiate(weaponSO.muzzleFlashEffect, raycastOrigin);
        bulletHitEffect = Instantiate(weaponSO.hitEffect, hitInfo.point, Quaternion.identity);
        tracerEffect = Instantiate(weaponSO.tracerEffect, raycastOrigin.position, Quaternion.identity);
    }

    public bool Fire()
    {
        if (!readyToFire) { return false; }
        
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;

        readyToFire = false;
        bulletCooldown = weaponSO.timeBetweenShots;
        bulletsRemainingInClip--;

        HandleWeaponEffects();

        if (!Physics.Raycast(ray, out hitInfo)) { return true; }

        if (!hitInfo.collider.gameObject.TryGetComponent<Health>(out Health targetHealth)) { return true; }
        // SWAP OUT HIT EFFECT WITH BLOOD SPLATTER EFFECT IF ALIEN IS HIT
        targetHealth.DealDamage(weaponSO.damageAmount);

        return true;
    }

    private void HandleWeaponEffects()
    {
        muzzleFlashEffect.Emit(1);

        bulletHitEffect.transform.position = hitInfo.point;
        bulletHitEffect.transform.forward = hitInfo.normal;
        bulletHitEffect.Emit(1);

        tracerEffect.transform.position = hitInfo.point;

        tracerEffect.emitting = false;
        tracerEffect.transform.position = raycastOrigin.position;
        tracerEffect.emitting = true;
    }
}
