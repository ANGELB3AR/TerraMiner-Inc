using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuOptions : MonoBehaviour
{
    [field:SerializeField] public ContextMenuButtonTypes[] contextMenuButtonTypes { get; private set; }
    [field: SerializeField] public Building[] buildingsAvailableToBuild { get; private set; } = null;
    
}
