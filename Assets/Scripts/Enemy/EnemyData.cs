using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 데이터 스크립터블 오브젝트 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    [Header("Basic Data")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private string _enemyName;
    public Enemy EnemyPrefab => _enemyPrefab;
    public string EnemyName => _enemyName;

    [Header("Controller Data")]
    [SerializeField] private EnemyControllerData _enemyControllerData;
    public EnemyControllerData EnemyControllerData => _enemyControllerData;

    [Header("Stats")]
    [SerializeField] private List<StatEntity<EnemyStatType>> _baseStats;
    [SerializeField] private List<StatEntity<EnemyStatType>> _increaseRates;
    public List<StatEntity<EnemyStatType>> BaseStats => _baseStats;
    public List<StatEntity<EnemyStatType>> IncreaseRates => _increaseRates;

    [Header("DropTable")]
    [SerializeField] private DropTableData _dropTable;
    public DropTableData DropTable => _dropTable;
}