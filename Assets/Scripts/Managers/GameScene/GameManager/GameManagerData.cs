using UnityEngine;

/// <summary>
/// 게임 매니저의 데이터 클래스
/// 라운드 시간과 목표 라운드 등 게임 진행에 필요한 데이터 포함
/// </summary>
[CreateAssetMenu(fileName = "GameManagerData", menuName = "SO/Game/GameManagerData", order = 0)]
public class GameManagerData : ScriptableObject
{
    [Header("Round Settings")]
    [SerializeField] private float _roundStartDelay = 3f;
    [SerializeField] private float _roundDuration = 60f;
    [SerializeField] private int _targetRounds = 10;
    public float RoundStartDelay => _roundStartDelay;
    public float RoundDuration => _roundDuration;
    public int TargetRounds => _targetRounds;

    [Header("Enemy Settings")]
    [SerializeField] private int _baseEnemySpawnCount = 5;
    [SerializeField] private int _enemySpawnCountIncrementPerRound = 3;
    [SerializeField] private float _baseEnemySpawnSpeed = 0.5f;
    [SerializeField] private float _enemySpawnSpeedIncrementPerRound = 0.1f;
    public int BaseEnemySpawnCount => _baseEnemySpawnCount;
    public int EnemySpawnCountIncrementPerRound => _enemySpawnCountIncrementPerRound;
    public float BaseEnemySpawnSpeed => _baseEnemySpawnSpeed;
    public float EnemySpawnSpeedIncrementPerRound => _enemySpawnSpeedIncrementPerRound;
}