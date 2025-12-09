/// <summary>
/// 게임 클리어 시의 상태 클래스
/// 목표 라운드를 클리어했을 시의 상태
/// </summary>
public class GameClearState : GameBaseState
{
    public GameClearState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        $"Game Cleared!".Log();
        //TODO: 게임 클리어 UI 표시 및 기타 처리
    }

    public override void Update() { }

    public override void Exit() { }
}
