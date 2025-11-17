using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 공격 컴포넌트
/// 무기를 장착하고 공격 명령을 받아 공격 실행
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    private List<Weapon> _weapons = new();
    public int WeaponCount => _weapons.Count;

    public void AddWeapon(Weapon weapon)
    {
        _weapons.Add(weapon);
    }

    public bool RemoveWeapon(Weapon weapon)
    {
        return _weapons.Remove(weapon);
    }

    public void StartAttack()
    {
        foreach (var weapon in _weapons)
        {
            weapon.StartAttack();
        }
    }

    public void StopAttack()
    {
        foreach (var weapon in _weapons)
        {
            weapon.StopAttack();
        }
    }
}