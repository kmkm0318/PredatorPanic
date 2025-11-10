using UnityEngine;

/// <summary>
/// 무기 데이터 스크립터블 오브젝트
/// </summary>
public abstract class WeaponData : ScriptableObject
{
    [SerializeField] private Weapon _weaponPrefab;
    public Weapon WeaponPrefab => _weaponPrefab;
}