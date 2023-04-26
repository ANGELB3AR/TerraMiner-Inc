using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] Button contextMenuButtonPrefab = null;
    [SerializeField] Transform contextMenuButtonParent = null;
    [SerializeField] Canvas contextMenuCanvas = null;

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        DeactivateContextMenu();
    }

    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        if (!hit.transform.gameObject.TryGetComponent<ContextMenuOptions>(out ContextMenuOptions options)) { return; }
        Debug.Log("Hit something with options");
        foreach (ContextMenuButtonTypes buttonType in options.contextMenuButtonTypes)
        {
            Button contextMenuButtonInstance = Instantiate(contextMenuButtonPrefab, contextMenuButtonParent);
            ActivateContextMenu();
        }

    }

    void ActivateContextMenu()
    {
        contextMenuCanvas.enabled = true;
    }

    void DeactivateContextMenu()
    {
        contextMenuCanvas.enabled = false;
    }
}

public enum ContextMenuButtonTypes
{
    BuildButton,
    DestroyButton,
    UpgradeButton
}
