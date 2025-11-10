using UnityEngine;

/// <summary>
/// 무기 추상 클래스
/// 무기 데이터로 초기화되고 공격 기능을 정의
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    protected WeaponData WeaponData { get; private set; }

    public virtual void Init(WeaponData weaponData)
    {
        WeaponData = weaponData;
    }

    public abstract void Attack();
}