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
    public static event Action<EmployeeMovement> LeftClickHitEmployee;

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
                if (hit.collider.gameObject.GetComponent<Terrain>())
                {
                    Debug.Log("Hit terrain");
                    LeftClickHitTerrain?.Invoke(hit.point);
                }
                if (hit.collider.gameObject.TryGetComponent<EmployeeMovement>(out EmployeeMovement employee))
                {
                    LeftClickHitEmployee?.Invoke(employee);
                }
            }
            else
            {
                Debug.Log("Left click hit nothing");
            }
        }
        else
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    Debug.Log($"Right click hit {hit.transform.gameObject.name}");
                }
                else
                {
                    Debug.Log("Right click hit nothing");
                }
            }
        }
    }
}
