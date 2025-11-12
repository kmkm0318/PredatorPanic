using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public float CurrentHealth { get; private set; }

    public float MaxHealth { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnDamaged;
    public event Action OnDeath;

    public void Init(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void Heal(float amount)
    {
        float healAmount = Mathf.Clamp(amount, 0, MaxHealth - CurrentHealth);

        CurrentHealth += healAmount;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        float damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);

        CurrentHealth -= damageTaken;
        OnDamaged?.Invoke(damageTaken);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }
    }
}