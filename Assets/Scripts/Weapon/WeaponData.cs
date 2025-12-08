using UnityEngine;

/// <summary>
/// 무기 데이터 스크립터블 오브젝트
/// </summary>
public abstract class WeaponData : ScriptableObject
{
    [Header("Basic Weapon Data")]
    [SerializeField] private string _weaponName;
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _basePrice;
    public string WeaponName => _weaponName;
    public Weapon WeaponPrefab => _weaponPrefab;
    public Sprite Icon => _icon;
    public int BasePrice => _basePrice;
}