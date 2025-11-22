using System;
using UnityEngine;

/// <summary>
/// 체력 관리 클래스
/// 플레이어와 적의 체력을 관리
/// 방어력을 통해 데미지 감소
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    #region 체력, 방어력
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public float Defense { get; private set; }
    #endregion

    #region 이벤트
    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnDamaged;
    public event Action OnDeath;
    #endregion

    //초기화
    public void Init(float maxHealth, float defense)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Defense = defense;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    #region 현재 체력 변동
    public void Heal(float amount)
    {
        float healAmount = Mathf.Clamp(amount, 0, MaxHealth - CurrentHealth);

        CurrentHealth += healAmount;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        damage = CombatUtility.CalculateDefensedDamage(damage, Defense);

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
    #endregion

    #region 최대 체력 및 방어력 재설정
    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void SetDefense(float newDefense)
    {
        Defense = newDefense;
    }
    #endregion
}