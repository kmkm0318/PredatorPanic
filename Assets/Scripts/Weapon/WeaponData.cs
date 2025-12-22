using UnityEngine;

/// <summary>
/// 무기 데이터 스크립터블 오브젝트
/// </summary>
public abstract class WeaponData : ScriptableObject
{
    [Header("Basic Weapon Data")]
    [SerializeField] private string _weaponName;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private int _basePrice;
    public string WeaponName => _weaponName;
    public string Description => _description;
    public Sprite Icon => _icon;
    public Rarity Rarity => _rarity;
    public int BasePrice => _basePrice;

    /// <summary>
    /// 무기 객체 반환 함수
    /// </summary>
    public abstract Weapon GetWeapon();
    public abstract string GetDescription();
}