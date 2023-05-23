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
    }

    public void Fire()
    {
        if (!readyToFire) { return; }
        
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;

        readyToFire = false;
        bulletCooldown = weaponSO.timeBetweenShots;
        bulletsRemainingInClip--;

        if (!Physics.Raycast(ray, out hitInfo)) { return; }

        Debug.DrawLine(raycastOrigin.position, hitInfo.point, Color.red);

        if (!hitInfo.collider.gameObject.TryGetComponent<Health>(out Health targetHealth)) { return; }
        
        targetHealth.DealDamage(weaponSO.damageAmount);
    }
}
