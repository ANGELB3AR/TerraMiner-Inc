using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    Camera mainCamera;

    public static event Action LeftClickHitButton;
    public static event Action<Vector3> LeftClickHitTerrain;
    public static event Action<Employee> LeftClickHitEmployee;
    public static event Action LeftClickHitNothing;

    public static event Action<ContextMenuOptions> RightClickHitContextItem;
    public static event Action<ContextMenuOptions, Vector3> RightClickHitTerrain;
    public static event Action RightClickHitNothing;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    LeftClickHitButton?.Invoke();
                }
                else if (hit.collider.gameObject.GetComponent<Terrain>())
                {
                    LeftClickHitTerrain?.Invoke(hit.point);
                }
                else if (hit.collider.gameObject.TryGetComponent<Employee>(out Employee employee))
                {
                    LeftClickHitEmployee?.Invoke(employee);
                }
            }
            else
            {
                LeftClickHitNothing?.Invoke();
            }
        }
        else
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.GetComponent<Terrain>())
                    {
                        RightClickHitTerrain?.Invoke(hit.collider.gameObject.GetComponent<ContextMenuOptions>(), hit.point);
                    }
                    else if (hit.collider.gameObject.TryGetComponent<ContextMenuOptions>(out ContextMenuOptions options))
                    {
                        RightClickHitContextItem?.Invoke(options);
                    }
                }
                else
                {
                    RightClickHitNothing?.Invoke();
                }
            }
        }
    }
}
