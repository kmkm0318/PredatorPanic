using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 게임 플레이 상태
/// </summary>
public class GamePlayingState : GameBaseState
{
    public GamePlayingState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //플레이어 인풋으로 변경
        InputManager.Instance.ChangeInputMode(InputMode.Player);

        //이벤트 구독
        RegisterEvents();

        //적 스폰 코루틴 시작
        int enemySpawnCount = GameManager.GameManagerData.BaseEnemySpawnCount
        + (GameManager.CurrentRound - 1)
        * GameManager.GameManagerData.EnemySpawnCountIncrementPerRound;

        float enemySpawnSpeed = GameManager.GameManagerData.BaseEnemySpawnSpeed
        + (GameManager.CurrentRound - 1)
        * GameManager.GameManagerData.EnemySpawnSpeedIncrementPerRound;

        float enemySpawnInterval = 1f / enemySpawnSpeed;

        GameManager.EnemyManager.StartEnemySpawn(GameManager.Player.transform, enemySpawnCount, enemySpawnInterval);
    }

    public override void Update()
    {
        //라운드 타이머 감소
        GameManager.RoundTimer -= Time.deltaTime;

        //레벨업 시도 및 성공 시 상태 전환하지 않음
        if (GameManager.Player.TryLevelUp()) return;

        //라운드 타이머가 0 이하가 되면 라운드 클리어 상태로 전환
        if (GameManager.RoundTimer <= 0f)
        {
            ChangeState(Factory.RoundClear);
        }
    }

    public override void Exit()
    {
        //이벤트 구독 해제
        UnregisterEvents();

        //적 스폰 코루틴 중지
        GameManager.EnemyManager.StopEnemySpawn();
    }

    #region 이벤트 및 이벤트 구독, 해제
    private void RegisterEvents()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;

        inputActions.Player.Pause.performed += OnPause;

        GameManager.Player.OnLevelUpped += OnLevelUpped;
    }

    private void UnregisterEvents()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;

        inputActions.Player.Pause.performed -= OnPause;

        GameManager.Player.OnLevelUpped -= OnLevelUpped;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        ChangeState(Factory.Pause);
    }

    private void OnLevelUpped(int level)
    {
        Factory.LevelUpPreviousState = this;
        ChangeState(Factory.LevelUp);
    }
    #endregion
}