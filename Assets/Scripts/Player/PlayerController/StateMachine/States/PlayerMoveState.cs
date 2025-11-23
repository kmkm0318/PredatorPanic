/// <summary>
/// 플레이어가 움직이는 상태
/// 지상, 점프, 떨어지는 상태에서 이동 입력이 있을 때 진입
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController playerController, PlayerStateFactory factory) : base(playerController, factory) { }

    public override void Enter()
    {
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsMovingHash, true);
    }

    public override void Update()
    {
        PlayerController.MovementX = PlayerController.MoveInput.x;
        PlayerController.MovementZ = PlayerController.MoveInput.y;
        CheckChangeState();
    }

    public override void Exit()
    {
        PlayerController.MovementX = 0;
        PlayerController.MovementZ = 0;
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsMovingHash, false);
    }

    public override void CheckChangeState()
    {
        if (!PlayerController.IsMovePressed)
        {
            ChangeState(Factory.Idle);
        }
    }
}