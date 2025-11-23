/// <summary>
/// 게임 상점 상태
/// 상점에서 플레이어가 아이템 등을 구매할 수 있음
/// 현재는 기능이 없음
/// </summary>
public class GameShopState : GameBaseState
{
    public GameShopState(GameManager gameManager, GameStateFactory factory) : base(gameManager, factory) { }

    public override void Enter()
    {
        //현재는 아무 동작 없음
        //바로 다음 라운드 시작 상태로 전환
        ChangeState(Factory.RoundStart);
    }

    public override void Update() { }

    public override void Exit() { }
}
