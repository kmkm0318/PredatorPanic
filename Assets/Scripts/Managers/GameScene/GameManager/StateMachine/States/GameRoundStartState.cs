using UnityEngine;

/// <summary>
/// 라운드 시작 상태
/// </summary>
public class GameRoundStartState : GameBaseState
{
    private float _roundStartDelayTimer;
    public GameRoundStartState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //라운드 시작 딜레이 타이머 초기화
        _roundStartDelayTimer = GameManager.GameData.RoundStartDelay;

        //라운드 타이머 초기화
        GameManager.RoundTimer = GameManager.GameData.RoundDuration;

        //적 스폰 변수 설정
        int roundIdx = GameManager.CurrentRound - 1;

        int enemySpawnCount = GameManager.GameData.BaseEnemySpawnCount
        + roundIdx
        * GameManager.GameData.EnemySpawnCountIncrementPerRound;

        float enemySpawnSpeed = GameManager.GameData.BaseEnemySpawnSpeed
        + roundIdx
        * GameManager.GameData.EnemySpawnSpeedIncrementPerRound;

        float enemySpawnInterval = 1f / enemySpawnSpeed;

        int enemyLevel = roundIdx;

        GameManager.EnemyManager.SetRoundEnemyVariables(GameManager.Player.transform, enemySpawnCount, enemySpawnInterval, enemyLevel);
    }

    public override void Update()
    {
        //딜레이 타이머 감소
        _roundStartDelayTimer -= Time.deltaTime;

        //딜레이 타이머가 0 이하가 되면 플레이 상태로 전환
        if (_roundStartDelayTimer <= 0f)
        {
            ChangeState(Factory.Playing);
        }
    }

    public override void Exit() { }
}
