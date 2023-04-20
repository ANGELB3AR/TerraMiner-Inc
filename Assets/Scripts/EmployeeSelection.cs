using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmployeeSelection : MonoBehaviour
{
    [SerializeField] List<GameObject> selectedEmployees = new List<GameObject>();
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        //Move(hit.point);
    }
}
