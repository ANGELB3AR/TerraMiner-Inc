using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] Collider hitbox = null;

    public static event Action<Collider> OnHitboxActivated;
    public static event Action<Collider> OnHitboxDeactivated;

    public void ActivateHitbox()
    {
        OnHitboxActivated?.Invoke(hitbox);
    }

    public void DeactivateHitbox()
    {
        OnHitboxDeactivated?.Invoke(hitbox);
    }
}
