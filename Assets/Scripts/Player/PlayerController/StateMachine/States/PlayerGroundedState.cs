/// <summary>
/// 플레이어가 지상에 있는 상태
/// 점프나 떨어지는 상태에서 지상에 닿았을 때 진입
/// 떨어지는 상태로 전환할 때 코요테 타임 시작
/// </summary>
public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerController playerController, PlayerStateFactory factory) : base(playerController, factory) { }

    public override void Enter()
    {
        PlayerController.MovementY = PlayerController.PlayerControllerData.GroundedGravitySpeed;

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
        if (PlayerController.IsMovePressed)
        {
            SetSubState(Factory.Move);
        }
        else
        {
            SetSubState(Factory.Idle);
        }
    }

    public override void CheckChangeState()
    {
        if (!PlayerController.CharacterController.isGrounded)
        {
            //지상에서 떨어질 때 코요테 타임 시작
            PlayerController.StartCoyoteTimeCoroutine();
            PlayerController.StateMachine.ChangeState(Factory.Fall);
        }
        else if (PlayerController.IsJumpPressed && PlayerController.IsJumpBuffer)
        {
            PlayerController.StopJumpBufferCoroutine();
            PlayerController.StateMachine.ChangeState(Factory.Jump);
        }
    }
}