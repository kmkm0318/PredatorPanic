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
    public Player Player { get; private set; }
    protected bool IsAttacking { get; private set; }
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

    /// <summary>
    /// 무기 설명 반환 메서드
    /// 기본적으로 데이터의 설명을 그대로 반환합니다.
    /// 런타임에 동적으로 변경된 설명이 필요한 경우 오버라이드합니다.
    /// </summary>
    public virtual string GetDescription()
    {
        return WeaponData.Description;
    }
}