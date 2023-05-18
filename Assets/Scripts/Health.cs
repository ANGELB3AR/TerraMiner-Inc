using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    int currentHealth;

    public event Action<int, int> OnHealthUpdated;
    public event Action OnDied;

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        OnHealthUpdated?.Invoke(currentHealth, maxHealth);
    }

    public void DealDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthUpdated?.Invoke(currentHealth, maxHealth);

        if (currentHealth != 0) { return; }

        Die();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthUpdated?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        OnDied?.Invoke();
    }
}
