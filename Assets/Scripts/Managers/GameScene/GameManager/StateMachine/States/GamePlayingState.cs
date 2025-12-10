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

        //아이템 드랍 가능하게 설정
        GameManager.DropItemManager.CanDrop = true;

        //적 스폰 시작
        GameManager.EnemyManager.StartEnemySpawn();
    }

    public override void Update()
    {
        //레벨업 시도 및 성공 시 상태 전환하지 않음
        if (GameManager.Player.TryLevelUp()) return;

        //라운드 타이머 처리
        HandleRoundTimer();
    }

    private void HandleRoundTimer()
    {
        //보스 라운드인 경우 타이머 처리하지 않음
        if (GameManager.EnemyManager.IsBossRound) return;

        //라운드 타이머 감소
        GameManager.RoundTimer -= Time.deltaTime;

        //라운드 타이머가 0 이하가 되면 라운드 클리어 상태로 전환
        if (GameManager.RoundTimer <= 0f)
        {
            ChangeState(Factory.RoundClear);
        }
    }

    public override void Exit()
    {
        //입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.None);

        //이벤트 구독 해제
        UnregisterEvents();

        //아이템 드랍 불가능하게 설정
        GameManager.DropItemManager.CanDrop = false;

        //적 스폰 중지
        GameManager.EnemyManager.StopEnemySpawn();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;
        inputActions.Player.Pause.performed += OnPause;

        GameManager.Player.OnLevelUpped += OnLevelUpped;
        GameManager.EnemyManager.OnAllBossDeath += OnAllBossDead;
    }

    private void UnregisterEvents()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;
        inputActions.Player.Pause.performed -= OnPause;

        GameManager.Player.OnLevelUpped -= OnLevelUpped;
        GameManager.EnemyManager.OnAllBossDeath -= OnAllBossDead;
    }
    #endregion

    #region 이벤트 핸들러
    private void OnPause(InputAction.CallbackContext context)
    {
        //일시정지 상태로 전환
        ChangeState(Factory.Pause);
    }

    private void OnLevelUpped(int level)
    {
        //레벨업 상태로 전환하기 전 현재 상태를 저장
        Factory.LevelUpPreviousState = this;

        //레벨업 상태로 전환
        ChangeState(Factory.LevelUp);
    }

    private void OnAllBossDead()
    {
        //보스가 모두 사망했을 때 라운드 클리어 상태로 전환
        ChangeState(Factory.RoundClear);
    }
    #endregion
}