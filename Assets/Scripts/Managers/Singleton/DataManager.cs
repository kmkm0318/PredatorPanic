using UnityEngine;

/// <summary>
/// 데이터 매니저 싱글톤 클래스
/// </summary>
public class DataManager : Singleton<DataManager>
{
    [Header("Player Stat Type Data List")]
    [SerializeField] private PlayerStatTypeDataList _playerStatTypeDataList;
    public PlayerStatTypeDataList PlayerStatTypeDataList => _playerStatTypeDataList;

    [Header("Level Up Reward Data List")]
    [SerializeField] private LevelUpRewardDataList _levelUpRewardDataList;
    public LevelUpRewardDataList LevelUpRewardDataList => _levelUpRewardDataList;

    [Header("Weapon Data List")]
    [SerializeField] private WeaponDataList _weaponDataList;
    public WeaponDataList WeaponDataList => _weaponDataList;

    [Header("Item Data List")]
    [SerializeField] private ItemDataList _itemDataList;
    public ItemDataList ItemDataList => _itemDataList;

    [Header("Damage Text Data List")]
    [SerializeField] private DamageTextDataList _damageTextDataList;
    public DamageTextDataList DamageTextDataList => _damageTextDataList;

    [Header("Rarity Data List")]
    [SerializeField] private RarityDataList _rarityDataList;
    public RarityDataList RarityDataList => _rarityDataList;
}