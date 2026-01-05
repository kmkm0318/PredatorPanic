using UnityEngine;

/// <summary>
/// 데이터 매니저 싱글톤 클래스
/// </summary>
public class DataManager : Singleton<DataManager>
{
    [Header("Player Stat Type Data List")]
    [SerializeField] private PlayerStatTypeDataList _playerStatTypeDataList;
    public PlayerStatTypeDataList PlayerStatTypeDataList => _playerStatTypeDataList;

    [Header("Gun Stat Type Data List")]
    [SerializeField] private GunStatTypeDataList _gunStatTypeDataList;
    public GunStatTypeDataList GunStatTypeDataList => _gunStatTypeDataList;

    [Header("Level Up Reward Data List")]
    [SerializeField] private LevelUpRewardDataList _levelUpRewardDataList;
    public LevelUpRewardDataList LevelUpRewardDataList => _levelUpRewardDataList;

    [Header("Run Data List")]
    [SerializeField] private RunDataList _runDataList;
    public RunDataList RunDataList => _runDataList;

    [Header("Player Data List")]
    [SerializeField] private PlayerDataList _playerDataList;
    public PlayerDataList PlayerDataList => _playerDataList;

    [Header("Weapon Data List")]
    [SerializeField] private WeaponDataList _weaponDataList;
    public WeaponDataList WeaponDataList => _weaponDataList;

    [Header("Item Data List")]
    [SerializeField] private ItemDataList _itemDataList;
    public ItemDataList ItemDataList => _itemDataList;

    [Header("Evolution Data List")]
    [SerializeField] private EvolutionDataList _evolutionDataList;
    public EvolutionDataList EvolutionDataList => _evolutionDataList;

    [Header("Damage Text Data List")]
    [SerializeField] private DamageTextDataList _damageTextDataList;
    public DamageTextDataList DamageTextDataList => _damageTextDataList;

    [Header("Rarity Data List")]
    [SerializeField] private RarityDataList _rarityDataList;
    [SerializeField] private RarityWeightData _rarityWeightData;
    public RarityDataList RarityDataList => _rarityDataList;
    public RarityWeightData RarityWeightData => _rarityWeightData;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask _enemyLayerMask;
    public LayerMask EnemyLayerMask => _enemyLayerMask;
}