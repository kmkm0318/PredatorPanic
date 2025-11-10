using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }

    public event Action<float> OnDamaged;
    public event Action<Vector3> OnDeath;

    public void Init(EnemyData enemyData)
    {
        MaxHealth = enemyData.MaxHealth;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        float damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
        CurrentHealth -= damageTaken;

        if (damageTaken != 0f)
        {
            OnDamaged?.Invoke(damageTaken);
        }

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke(transform.position);
        }
    }
}