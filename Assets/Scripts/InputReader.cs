using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    Camera mainCamera;

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
                Debug.Log($"Left click hit {hit.transform.gameObject.name}");
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
