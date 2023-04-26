using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] Outline hoverOutline = null;

    private void OnMouseEnter()
    {
        hoverOutline.enabled = true;
    }

    private void OnMouseExit()
    {
        hoverOutline.enabled = false;
    }
}
