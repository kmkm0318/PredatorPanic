/// <summary>
/// 플레이어가 지상에 있는 상태
/// 점프나 떨어지는 상태에서 지상에 닿았을 때 진입
/// 떨어지는 상태로 전환할 때 코요테 타임 시작
/// </summary>
public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController owner) : base(owner) { }

    public override void Enter()
    {
        Owner.MovementY = Owner.PlayerControllerData.GroundedGravitySpeed;

        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        SubState?.Update();

        CheckChangeState();
    }

    public override void Exit()
    {

        SubState?.Exit();
    }

    public override void InitSubState()
    {
        if (Owner.IsMovePressed)
        {
            SetSubState(Owner.StateFactory.Move());
        }
        else
        {
            SetSubState(Owner.StateFactory.Idle());
        }
    }

    public override void CheckChangeState()
    {
        if (!Owner.CharacterController.isGrounded)
        {
            //지상에서 떨어질 때 코요테 타임 시작
            Owner.StartCoyoteTimeCoroutine();
            Owner.StateMachine.ChangeState(Owner.StateFactory.Fall());
        }
        else if (Owner.IsJumpPressed && Owner.IsJumpBuffer)
        {
            Owner.IsJumpBuffer = false;
            Owner.StateMachine.ChangeState(Owner.StateFactory.Jump());
        }
    }
}