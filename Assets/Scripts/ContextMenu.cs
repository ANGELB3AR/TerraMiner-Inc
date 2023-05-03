using System;
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

    private void OnEnable()
    {
        InputReader.RightClickHitContextItem += InputReader_RightClickHitContextItem;
        InputReader.RightClickHitTerrain += InputReader_RightClickHitTerrain;

        InputReader.RightClickHitNothing += DeactivateContextMenu;
        InputReader.LeftClickHitNothing += DeactivateContextMenu;
        InputReader.LeftClickHitEmployee += InputReader_LeftClickHitEmployee;
        InputReader.LeftClickHitTerrain += InputReader_LeftClickHitTerrain;
    }

    private void OnDisable()
    {
        InputReader.RightClickHitContextItem -= InputReader_RightClickHitContextItem;
        InputReader.RightClickHitTerrain -= InputReader_RightClickHitTerrain;

        InputReader.RightClickHitNothing -= DeactivateContextMenu;
        InputReader.LeftClickHitNothing -= DeactivateContextMenu;
        InputReader.LeftClickHitEmployee += InputReader_LeftClickHitEmployee;
        InputReader.LeftClickHitTerrain += InputReader_LeftClickHitTerrain;
    }

    private void InputReader_RightClickHitContextItem(ContextMenuOptions options)
    {
        ClearContextMenu();

        contextMenuTitleText.text = options.contextMenuTitle;

        foreach (ContextMenuButtonTypes buttonType in options.contextMenuButtonTypes)
        {
            Button contextMenuButtonInstance = Instantiate(contextMenuButtonPrefab, contextMenuButtonParent);

            switch (buttonType)
            {
                case ContextMenuButtonTypes.BuildButton:
                    contextMenuButtonInstance.onClick.AddListener(() => GenerateBuildMenu(options.buildingsAvailableToBuild, options.transform.position));
                    contextMenuButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = "BUILD";
                    break;
                case ContextMenuButtonTypes.DestroyButton:
                    contextMenuButtonInstance.onClick.AddListener(() => buildingManager.DestroyBuilding(options.GetComponent<Building>()));
                    contextMenuButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = "DESTROY";
                    break;
                case ContextMenuButtonTypes.UpgradeButton:
                    break;
                default:
                    break;
            }
        }
        ActivateContextMenu();
    }

    private void InputReader_RightClickHitTerrain(ContextMenuOptions options, Vector3 position)
    {
        ClearContextMenu();

        contextMenuTitleText.text = options.contextMenuTitle;

        Button contextMenuButtonInstance = Instantiate(contextMenuButtonPrefab, contextMenuButtonParent);

        contextMenuButtonInstance.onClick.AddListener(() => GenerateBuildMenu(options.buildingsAvailableToBuild, position));
        contextMenuButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = "BUILD";

        ActivateContextMenu();
    }

    private void InputReader_LeftClickHitTerrain(Vector3 obj)
    {
        DeactivateContextMenu();
    }

    private void InputReader_LeftClickHitEmployee(Employee obj)
    {
        DeactivateContextMenu();
    }


    void GenerateBuildMenu(Building[] buildingsAvailableToBuild, Vector3 locationToPlaceBuilding)
    {
        ClearContextMenu();
        contextMenuTitleText.text = "Build Menu";
        foreach (Building building in buildingsAvailableToBuild)
        {
            Button buildingMenuButtonInstance = Instantiate(contextMenuButtonPrefab, contextMenuButtonParent);
            buildingMenuButtonInstance.onClick.AddListener(() => buildingManager.PlaceBuilding(building, locationToPlaceBuilding));
            buildingMenuButtonInstance.GetComponentInChildren<TextMeshProUGUI>().text = building.name;
        }
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
