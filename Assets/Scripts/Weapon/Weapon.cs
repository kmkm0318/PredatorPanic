using System;
using UnityEngine;

/// <summary>
/// 무기 추상 클래스
/// 무기 데이터로 초기화되고 공격 기능을 정의
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    #region 데이터
    public WeaponData WeaponData { get; private set; }
    #endregion

    #region 변수들
    protected Player Player { get; private set; }
    protected bool IsAttacking { get; private set; }
    #endregion

    #region 이벤트
    public event Action<IDamageable, float> OnHit;
    public event Action<IDamageable> OnKill;
    #endregion

    public virtual void Init(WeaponData weaponData, Player player)
    {
        WeaponData = weaponData;
        Player = player;
    }

    #region 공격 시작, 중지 함수
    public virtual void StartAttack()
    {
        IsAttacking = true;
    }

    public virtual void StopAttack()
    {
        IsAttacking = false;
    }
    #endregion

    #region 이벤트 발생 함수
    protected void HitTarget(IDamageable target, float damage)
    {
        OnHit?.Invoke(target, damage);
    }

    protected void KillTarget(IDamageable target)
    {
        OnKill?.Invoke(target);
    }
    #endregion
}