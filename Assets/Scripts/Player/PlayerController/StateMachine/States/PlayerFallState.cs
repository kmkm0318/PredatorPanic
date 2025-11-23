using UnityEngine;

/// <summary>
/// 플레이어가 떨어지는 상태
/// 점프 후 떨어지기 시작하거나 점프 키를 떼었을 때 진입
/// 지상에서 아래로 떨어질 때 진입
/// 코요테 타임일 시 점프 가능
/// </summary>
public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerController playerController, PlayerStateFactory factory) : base(playerController, factory) { }

    public override void Enter()
    {
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsFallingHash, true);
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        PlayerController.MovementY += PlayerController.Gravity * PlayerController.PlayerControllerData.FallGravityMultiplier * Time.deltaTime;
        PlayerController.MovementY = Mathf.Max(PlayerController.MovementY, PlayerController.PlayerControllerData.FallSpeedMin);
        SubState?.Update();

        CheckChangeState();
    }

    public override void Exit()
    {
        SubState?.Exit();
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsFallingHash, false);
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
        if (PlayerController.CharacterController.isGrounded)
        {
            PlayerController.StateMachine.ChangeState(Factory.Grounded);
        }
        else if (PlayerController.IsJumpPressed && PlayerController.IsJumpBuffer && PlayerController.IsCoyoteTime)
        {
            //점프 버퍼와 코요테 타임을 모두 만족할 때 점프
            PlayerController.StopJumpBufferCoroutine();
            PlayerController.StopCoyoteTimeCoroutine();
            PlayerController.StateMachine.ChangeState(Factory.Jump);
        }
    }
}