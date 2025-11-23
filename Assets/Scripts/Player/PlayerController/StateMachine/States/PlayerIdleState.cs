/// <summary>
/// 플레이어가 가만히 있는 상태
/// 지상, 점프, 떨어지는 상태에서 이동 입력이 없을 때 진입
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController playerController, PlayerStateFactory factory) : base(playerController, factory) { }

    public override void Enter()
    {

    }

    public override void Update()
    {
        CheckChangeState();
    }

    public override void Exit()
    {

    }

    public override void CheckChangeState()
    {
        if (PlayerController.IsMovePressed)
        {
            ChangeState(Factory.Move);
        }
    }
}