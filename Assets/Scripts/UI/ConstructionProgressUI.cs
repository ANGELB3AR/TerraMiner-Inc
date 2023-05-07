using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionProgressUI : MonoBehaviour
{
    [SerializeField] Slider constructionProgressSlider = null;
    [SerializeField] Material originalMaterial = null;
    [SerializeField] Material underConstructionMaterial = null;

    Building building = null;
    Camera mainCamera = null;
    List<Renderer> renderers = new List<Renderer>();

    private void Awake()
    {
        mainCamera = Camera.main;
        building = GetComponentInParent<Building>();

        constructionProgressSlider.value = 0f;
        constructionProgressSlider.maxValue = building.GetMaxConstructionProgress();

        foreach (Renderer renderer in building.GetComponentsInChildren<Renderer>())
        {
            renderers.Add(renderer);
        }

        originalMaterial = renderers[0].material;

        foreach (Renderer renderer in renderers)
        {
            renderer.material = underConstructionMaterial;
        }
    }

    private void Update()
    {
        constructionProgressSlider.value = building.GetCurrentConstructionProgress();

        if (!building.GetConstructionCompleteStatus()) { return; }

        foreach (Renderer renderer in renderers)
        {
            renderer.material = originalMaterial;
        }

        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.forward = mainCamera.transform.forward;
    }
}
