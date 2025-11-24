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
