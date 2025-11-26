using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 공격 컴포넌트
/// 무기를 장착하고 공격 명령을 받아 공격 실행
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    //장착된 무기들
    public List<Weapon> Weapons { get; private set; } = new();

    #region 이벤트
    public event Action<IDamageable, float> OnHit; //적중 이벤트
    public event Action<IDamageable> OnKill; //킬 이벤트
    #endregion

    //Weapon으로 무기 추가
    public void AddWeapon(Weapon weapon)
    {
        Weapons.Add(weapon);
        weapon.OnHit += HandleOnHit;
        weapon.OnKill += HandleOnKill;
    }

    //Weapon으로 무기 제거. 무기 인덱스 반환
    public int RemoveWeapon(Weapon weapon)
    {
        int idx = Weapons.IndexOf(weapon);
        if (idx >= 0)
        {
            weapon.OnHit -= HandleOnHit;
            weapon.OnKill -= HandleOnKill;
            Weapons.RemoveAt(idx);
        }
        return idx;
    }

    #region 이벤트 핸들러
    private void HandleOnHit(IDamageable target, float damage)
    {
        OnHit?.Invoke(target, damage);
    }

    private void HandleOnKill(IDamageable target)
    {
        OnKill?.Invoke(target);
    }
    #endregion

    //공격 시작
    public void StartAttack()
    {
        foreach (var weapon in Weapons)
        {
            weapon.StartAttack();
        }
    }

    //공격 종료
    public void StopAttack()
    {
        foreach (var weapon in Weapons)
        {
            weapon.StopAttack();
        }
    }
}