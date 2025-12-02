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
        //중력 속도 초기화
        PlayerController.MovementY = PlayerController.PlayerControllerData.GroundedGravitySpeed;

        //공중 점프 가능 횟수 초기화
        PlayerController.ResetJumpRemain();

        //서브 상태 초기화
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        //서브 상태 업데이트
        SubState?.Update();

        //상태 전환 체크
        CheckChangeState();
    }

    public override void Exit()
    {
        //서브 상태 종료
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
        if (!PlayerController.IsGrounded)
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