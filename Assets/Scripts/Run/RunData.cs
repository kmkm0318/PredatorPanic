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
    [SerializeField] private float _baseEnemyStatRate = 1f;
    [SerializeField] private float _enemyStatIncreaseRate = 1.25f;
    public EnemyTableDataList EnemyTableDataList => _enemyTableDataList;
    public float BaseEnemySpawnCount => _baseEnemySpawnCount;
    public float EnemySpawnCountIncreaseRate => _enemySpawnCountIncreaseRate;
    public float BaseEnemySpawnSpeed => _baseEnemySpawnSpeed;
    public float EnemySpawnSpeedIncreaseRate => _enemySpawnSpeedIncreaseRate;
    public float BaseEnemyStatRate => _baseEnemyStatRate;
    public float EnemyStatIncreaseRate => _enemyStatIncreaseRate;

    public string GetDescription()
    {
        return
        $"목표 라운드: {_enemyTableDataList.EnemyTableDatas.Count}\n" +
        $"기본 적 수: {_baseEnemySpawnCount}\n" +
        $"적 수 증가율: {_enemySpawnCountIncreaseRate}\n" +
        $"기본 적 출현 속도: {_baseEnemySpawnSpeed}\n" +
        $"적 출현 속도 증가율: {_enemySpawnSpeedIncreaseRate}\n" +
        $"기본 적 능력치 배율: {_baseEnemyStatRate}\n" +
        $"적 능력치 배율 증가율: {_enemyStatIncreaseRate}";
    }
}