using UnityEngine;

/// <summary>
/// 무기 데이터 스크립터블 오브젝트
/// </summary>
public abstract class WeaponData : ScriptableObject
{
    [Header("Basic Weapon Data")]
    [SerializeField] private string _weaponName;
    [SerializeField] private string _description;
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private int _basePrice;
    public string WeaponName => _weaponName;
    public string Description => _description;
    public Weapon WeaponPrefab => _weaponPrefab;
    public Sprite Icon => _icon;
    public Rarity Rarity => _rarity;
    public int BasePrice => _basePrice;
}