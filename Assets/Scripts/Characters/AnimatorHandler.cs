using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField] Hitbox hitbox = null;

    public void ActivateHitbox()
    {
        hitbox.ActivateHitbox();
    }

    public void DeactivateHitbox()
    {
        hitbox.DeactivateHitbox();
    }
}
