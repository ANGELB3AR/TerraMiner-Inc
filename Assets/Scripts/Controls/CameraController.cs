using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] AnimationCurve panSpeedCurve = null;
    [SerializeField] float zoomSpeed = 3f;
    [SerializeField] Vector2 zoomLimit = Vector2.zero;
    [SerializeField] float minPanSpeed = 1f;
    [SerializeField] float maxPanSpeed = 10f;


    CinemachineInputProvider inputProvider;
    CinemachineVirtualCamera vCamMain;
    Transform cameraTransform;
    float panTime = 0;

    private void Awake()
    {
        inputProvider = GetComponentInChildren<CinemachineInputProvider>();
        vCamMain = GetComponentInChildren<CinemachineVirtualCamera>();
        cameraTransform = vCamMain.VirtualCameraGameObject.transform;
    }

    private void Update()
    {
        float x = inputProvider.GetAxisValue(0);
        float y = inputProvider.GetAxisValue(1);
        float z = inputProvider.GetAxisValue(2);

        if (x != 0 || y != 0)
        {
            PanScreen(x, y);
        }

        if (z != 0)
        {
            ZoomScreen(z);
        }
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
            direction.z += 1;
        }
        else if (y <= Screen.height * 0.05f)
        {
            direction.z -= 1;
        }

        if (x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }

        return direction;
    }
}
