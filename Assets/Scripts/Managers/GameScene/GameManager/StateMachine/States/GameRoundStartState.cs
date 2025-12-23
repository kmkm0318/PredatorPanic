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

        //라운드 수 증가
        GameManager.CurrentRound++;

        //적 스폰 변수 설정
        GameManager.EnemyManager.SetRoundEnemyVariables();

        //보스 라운드가 아닐 시
        if (!GameManager.EnemyManager.IsBossRound)
        {
            //라운드 타이머 초기화
            GameManager.RoundTimer = GameManager.GameData.RoundDuration;

            //라운드 타이머 UI 표시
            GameManager.GameUIManager.RoundPresenter.ShowRoundTimerText(true);
        }

        //플레이어 입력 모드 변경
        InputManager.Instance.ChangeInputMode(InputMode.Player);
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

    public override void Exit()
    {
        //플레이어 입력 모드 없음으로 변경
        InputManager.Instance.ChangeInputMode(InputMode.None);
    }
}
