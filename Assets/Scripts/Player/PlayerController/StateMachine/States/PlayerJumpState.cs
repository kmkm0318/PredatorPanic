using UnityEngine;

/// <summary>
/// 플레이어가 점프하는 상태
/// 지상에서 점프 키를 눌렀을 때 진입
/// </summary>
public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController playerController, PlayerStateFactory factory) : base(playerController, factory) { }

    public override void Enter()
    {
        //점프 애니메이션 재생 및 초기 점프 속도 설정
        PlayerController.PlayerVisual.Animator.SetBool(PlayerVisual.IsJumpingHash, true);
        PlayerController.MovementY = PlayerController.InitialJumpSpeed;

        //서브 상태 초기화
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        //중력 적용
        PlayerController.MovementY += PlayerController.PlayerControllerData.Gravity * Time.deltaTime;

        //서브 상태 업데이트
        SubState?.Update();

        //상태 전환 체크
        CheckChangeState();
    }

    public override void Exit()
    {
        //서브 상태 종료
        SubState?.Exit();

        //점프 애니메이션 종료
        PlayerController.PlayerVisual.Animator.SetBool(PlayerVisual.IsJumpingHash, false);
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
        if (PlayerController.IsGrounded)
        {
            PlayerController.StateMachine.ChangeState(Factory.Grounded);
        }
        else if (PlayerController.MovementY <= 0 || !PlayerController.IsJumpPressed)
        {
            PlayerController.StateMachine.ChangeState(Factory.Fall);
        }
    }
}