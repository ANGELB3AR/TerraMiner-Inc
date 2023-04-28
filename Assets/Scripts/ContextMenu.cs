using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] Button contextMenuButtonPrefab = null;
    [SerializeField] Transform contextMenuButtonParent = null;
    [SerializeField] Canvas contextMenuCanvas = null;
    [SerializeField] TextMeshProUGUI contextMenuTitleText = null;
    [SerializeField] BuildingManager buildingManager = null;

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

        ClearContextMenu();

        contextMenuTitleText.text = options.contextMenuTitle;

        foreach (ContextMenuButtonTypes buttonType in options.contextMenuButtonTypes)
        {
            Button contextMenuButtonInstance = Instantiate(contextMenuButtonPrefab, contextMenuButtonParent);

            switch (buttonType)
            {
                case ContextMenuButtonTypes.BuildButton:
                    contextMenuButtonInstance.onClick.AddListener(() => buildingManager.PlaceBuilding(options.buildingsAvailableToBuild, options.transform));
                    contextMenuButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = "BUILD";
                    break;
                case ContextMenuButtonTypes.DestroyButton:
                    break;
                case ContextMenuButtonTypes.UpgradeButton:
                    break;
                default:
                    break;
            }
        }
        ActivateContextMenu();
    }

    void ActivateContextMenu()
    {
        contextMenuCanvas.enabled = true;
    }

    void DeactivateContextMenu()
    {
        contextMenuCanvas.enabled = false;

        ClearContextMenu();
    }

    private void ClearContextMenu()
    {
        for (int i = 1; i < contextMenuButtonParent.childCount; i++)
        {
            Destroy(contextMenuButtonParent.GetChild(i).gameObject);
        }
    }
}

public enum ContextMenuButtonTypes
{
    BuildButton,
    DestroyButton,
    UpgradeButton
}
