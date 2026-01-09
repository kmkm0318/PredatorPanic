using UnityEngine;

/// <summary>
/// 런 데이터
/// 적 테이블, 난이도 등에 대한 정보를 포함
/// </summary>
[CreateAssetMenu(fileName = "RunData", menuName = "SO/Run/RunData", order = 0)]
public class RunData : ScriptableObject, IBasicData
{
    [Header("Basic Data")]
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private int _basePrice;
    public string ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public Rarity Rarity => _rarity;
    public int BasePrice => _basePrice;

    [Header("Enemy Settings")]
    [SerializeField] private EnemyTableDataList _enemyTableDataList;
    [SerializeField] private float _baseEnemySpawnCount = 5f;
    [SerializeField] private float _enemySpawnCountIncreaseRate = 1.1f;
    [SerializeField] private float _baseEnemySpawnSpeed = 0.5f;
    [SerializeField] private float _enemySpawnSpeedIncreaseRate = 1.1f;
    [SerializeField] private float _baseEnemyHealthRate = 1f;
    [SerializeField] private float _baseEnemyDamageRate = 1f;
    [SerializeField] private float _baseEnemySpeedRate = 1f;
    [SerializeField] private float _enemyHealthIncreaseRate = 1.5f;
    [SerializeField] private float _enemyDamageIncreaseRate = 1.5f;
    [SerializeField] private float _enemySpeedIncreaseRate = 1.1f;
    public EnemyTableDataList EnemyTableDataList => _enemyTableDataList;
    public float BaseEnemySpawnCount => _baseEnemySpawnCount;
    public float EnemySpawnCountIncreaseRate => _enemySpawnCountIncreaseRate;
    public float BaseEnemySpawnSpeed => _baseEnemySpawnSpeed;
    public float EnemySpawnSpeedIncreaseRate => _enemySpawnSpeedIncreaseRate;
    public float BaseEnemyHealthRate => _baseEnemyHealthRate;
    public float BaseEnemyDamageRate => _baseEnemyDamageRate;
    public float BaseEnemySpeedRate => _baseEnemySpeedRate;
    public float EnemyHealthIncreaseRate => _enemyHealthIncreaseRate;
    public float EnemyDamageIncreaseRate => _enemyDamageIncreaseRate;
    public float EnemySpeedIncreaseRate => _enemySpeedIncreaseRate;

    public string GetDescription()
    {
        return
        $"기본 적 수: <color=green>{_baseEnemySpawnCount}</color>\n" +
        $"기본 적 출현 속도: <color=green>{_baseEnemySpawnSpeed}</color>\n" +
        $"기본 적 체력: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _baseEnemyHealthRate)}\n" +
        $"기본 적 공격력: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _baseEnemyDamageRate)}\n" +
        $"기본 적 이동속도: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _baseEnemySpeedRate)}\n" +
        $"라운드 당 적 수: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _enemySpawnCountIncreaseRate)}\n" +
        $"라운드 당 적 출현 속도: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _enemySpawnSpeedIncreaseRate)}\n" +
        $"라운드 당 적 체력: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _enemyHealthIncreaseRate)}\n" +
        $"라운드 당 적 공격력: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _enemyDamageIncreaseRate)}\n" +
        $"라운드 당 적 이동속도: {StringUtility.GetModifierDescription(StatModifierType.PercentMult, _enemySpeedIncreaseRate)}";
    }
}