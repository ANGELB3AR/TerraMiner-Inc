using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] AnimationCurve panSpeedCurve = null;
    [SerializeField] float zoomSpeed = 3f;
    [SerializeField] Vector2 zoomLimit = Vector2.zero;
    [SerializeField] float minPanSpeed = 1f;
    [SerializeField] float maxPanSpeed = 10f;
    [SerializeField] float rotationSpeed = 3f;

    CinemachineVirtualCamera vCamMain;
    Transform cameraTransform;
    float panTime = 0;
    Vector2 panInput = new Vector2();
    float zoomInput = 0f;
    bool isRotatingCamera = false;

    private void Awake()
    {
        vCamMain = GetComponentInChildren<CinemachineVirtualCamera>();
        cameraTransform = vCamMain.VirtualCameraGameObject.transform;
    }

    private void Update()
    {
        isRotatingCamera = (Mouse.current.rightButton.IsActuated(0.5f)) ? true : false;

        if (panInput.x != 0 || panInput.y != 0)
        {
            PanScreen(panInput.x, panInput.y);
        }

        if (zoomInput != 0)
        {
            ZoomScreen(zoomInput);
        }

        if (isRotatingCamera)
        {
            RotateScreen();
        }
    }

    void OnPanCamera(InputValue value)
    {
        panInput = value.Get<Vector2>();
    }

    void OnZoomCamera(InputValue value)
    {
        zoomInput = value.Get<float>();
    }

    private void ZoomScreen(float increment)
    {
        float currentZoom = cameraTransform.position.y;
        float targetZoom = Mathf.Clamp(currentZoom + increment, zoomLimit.x, zoomLimit.y);

        Vector3 newPosition = new Vector3(cameraTransform.position.x, targetZoom, cameraTransform.position.z);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, newPosition, zoomSpeed * Time.deltaTime);
    }

    void PanScreen(float x, float y)
    {
        Vector3 direction = PanDirection(x, y);

        if (direction == Vector3.zero)
        {
            panTime = 0f;
        }
        
        panTime += Time.deltaTime;

        float currentZoom = cameraTransform.position.y;
        float zoomLevel = Mathf.InverseLerp(zoomLimit.x, zoomLimit.y, currentZoom);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
                                                cameraTransform.position + direction,
                                                panSpeedCurve.Evaluate(panTime) * Mathf.Lerp(minPanSpeed, maxPanSpeed, zoomLevel) * Time.deltaTime);
    }

    private Vector3 PanDirection(float x, float y)
    {
        Vector3 direction = Vector3.zero;

        if (y >= Screen.height * 0.95f)
        {
            direction -= cameraTransform.forward;   // Inverted direction to fix obscure bug
        }
        else if (y <= Screen.height * 0.05f)
        {
            direction += cameraTransform.forward;   // Inverted direction to fix obscure bug
        }

        if (x >= Screen.width * 0.95f)
        {
            direction += cameraTransform.right;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction -= cameraTransform.right;
        }

        direction.z = direction.y;
        direction.y = 0f;

        return direction.normalized;
    }

    private void RotateScreen()
    {
        Quaternion currentRotation = cameraTransform.rotation;
        float rotationValue = Mathf.Clamp(RotateDirection(), -1f, 1f);
        Quaternion rotationDelta = Quaternion.Euler(0f, rotationValue, 0f);

        Quaternion targetRotation = rotationDelta * currentRotation;

        cameraTransform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private float RotateDirection()
    {
        float direction = 0f;

        if (panInput.x > Screen.width * 0.5f)
        {
            direction += 1f;
        }
        else if (panInput.x < Screen.width * 0.5f)
        {
            direction -= 1f;
        }

        return direction;
    }
}
