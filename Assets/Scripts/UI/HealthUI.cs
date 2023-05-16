using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Slider healthBarSlider = null;
    
    Health health;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
    }

    private void OnEnable()
    {
        health.OnHealthUpdated += Health_OnHealthUpdated;
    }

    private void OnDisable()
    {
        health.OnHealthUpdated -= Health_OnHealthUpdated;
    }

    private void Health_OnHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarSlider.value = currentHealth;
        healthBarSlider.maxValue = maxHealth;
    }
}
