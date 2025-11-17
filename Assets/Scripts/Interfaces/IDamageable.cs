using System;

/// <summary>
/// 대미지를 받거나 힐을 할 수 있는 객체를 위한 인터페이스
/// </summary>
public interface IDamageable
{
    //현재, 최대 체력
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    public float Defense { get; }

    //대미지 및 사망 이벤트
    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnDamaged;
    public event Action OnDeath;

    //체력 변동 메서드
    void TakeDamage(float damage);
    void Heal(float amount);
}