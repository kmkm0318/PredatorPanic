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
    [SerializeField, TextArea(3, 10)] private string _description;
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
    [SerializeField] private int _baseEnemySpawnCount = 4;
    [SerializeField] private float _enemySpawnCountIncreaseRate = 1.25f;
    [SerializeField] private float _baseEnemySpawnSpeed = 0.5f;
    [SerializeField] private float _enemySpawnSpeedIncreaseRate = 1.25f;
    [SerializeField] private float _baseEnemyStatGrowthRate = 1f;
    [SerializeField] private float _enemyStatGrowthIncreaseRate = 1.25f;
    public EnemyTableDataList EnemyTableDataList => _enemyTableDataList;
    public int BaseEnemySpawnCount => _baseEnemySpawnCount;
    public float EnemySpawnCountIncreaseRate => _enemySpawnCountIncreaseRate;
    public float BaseEnemySpawnSpeed => _baseEnemySpawnSpeed;
    public float EnemySpawnSpeedIncreaseRate => _enemySpawnSpeedIncreaseRate;
    public float BaseEnemyStatGrowthRate => _baseEnemyStatGrowthRate;
    public float EnemyStatGrowthIncreaseRate => _enemyStatGrowthIncreaseRate;
}