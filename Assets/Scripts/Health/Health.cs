using System;
using UnityEngine;

/// <summary>
/// 체력 관리 클래스
/// 플레이어와 적의 체력을 관리
/// 방어력을 통해 데미지 감소
/// </summary>
public class Health : MonoBehaviour
{
    #region 체력, 방어력
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public bool IsDead { get; private set; }
    #endregion

    #region 이벤트
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    #endregion

    //초기화
    public virtual void Init(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        IsDead = false;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    #region 현재 체력 변동
    public virtual void Heal(float amount)
    {
        //죽은 상태인 경우 패스
        if (IsDead) return;

        //회복량이 0 이하인 경우 패스
        if (amount <= 0f) return;

        //체력 회복
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);

        //이벤트 호출
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    // 데미지 입기
    public virtual void TakeDamage(float damage)
    {
        //죽은 상태인 경우 패스
        if (IsDead) return;

        //체력 감소
        CurrentHealth -= damage;

        //이벤트 호출
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        //체력이 0 이하일 시 사망 처리
        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0;
            IsDead = true;

            //이벤트 호출
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
    #endregion
}