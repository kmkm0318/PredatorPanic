/// <summary>
/// 처음 게임 씬 시작 시의 로딩 상태
/// </summary>
public class GameLoadingState : GameBaseState
{
    public GameLoadingState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //게임 매니저 초기화
        GameManager.Init();

        //바로 라운드 시작 상태로 전환
        ChangeState(Factory.RoundStart);
    }

    public override void Update() { }

    public override void Exit() { }
}
