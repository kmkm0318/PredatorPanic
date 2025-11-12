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
    [SerializeField] private List<EnemyStatEntity> _initialStats;
    public List<EnemyStatEntity> InitialStats => _initialStats;
}