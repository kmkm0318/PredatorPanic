using System;
using UnityEngine;

/// <summary>
/// 대미지를 받을 수 있는 객체를 위한 인터페이스
/// </summary>
public interface IDamageable
{
    //현재, 최대 체력
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    //대미지 및 사망 이벤트
    public event Action<float> OnDamaged;
    public event Action<Vector3> OnDeath;

    //대미지를 입히는 메서드
    void TakeDamage(float damage);
}