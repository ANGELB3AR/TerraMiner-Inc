using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] Collider hitbox = null;

    public static event Action<Collider> OnHitboxActivated;
    public static event Action<Collider> OnHitboxDeactivated;
    public event Action OnAttackAttempted;

    public void ActivateHitbox()
    {
        OnHitboxActivated?.Invoke(hitbox);
    }

    public void DeactivateHitbox()
    {
        OnHitboxDeactivated?.Invoke(hitbox);
    }


    public void AttemptAttack()
    {
        OnAttackAttempted?.Invoke();
    }

}
