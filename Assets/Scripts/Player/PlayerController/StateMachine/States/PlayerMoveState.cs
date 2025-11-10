/// <summary>
/// 플레이어가 움직이는 상태
/// 지상, 점프, 떨어지는 상태에서 이동 입력이 있을 때 진입
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController owner) : base(owner) { }

    public override void Enter()
    {
        Owner.PlayerVisual.Animator.SetBool(Owner.PlayerVisual.IsMovingHash, true);
    }

    public override void Update()
    {
        Owner.MovementX = Owner.MoveInput.x;
        Owner.MovementZ = Owner.MoveInput.y;
        CheckChangeState();
    }

    public override void Exit()
    {
        Owner.MovementX = 0;
        Owner.MovementZ = 0;
        Owner.PlayerVisual.Animator.SetBool(Owner.PlayerVisual.IsMovingHash, false);
    }

    public override void InitSubState()
    {

    }

    public override void CheckChangeState()
    {
        if (!Owner.IsMovePressed)
        {
            ChangeState(Owner.StateFactory.Idle());
        }
    }
}