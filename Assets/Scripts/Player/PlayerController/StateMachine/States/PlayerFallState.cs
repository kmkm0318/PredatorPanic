using UnityEngine;

/// <summary>
/// 플레이어가 떨어지는 상태
/// 점프 후 떨어지기 시작하거나 점프 키를 떼었을 때 진입
/// 지상에서 아래로 떨어질 때 진입
/// 코요테 타임일 시 점프 가능
/// </summary>
public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerController owner) : base(owner) { }

    public override void Enter()
    {
        Owner.PlayerVisual.Animator.SetBool(Owner.PlayerVisual.IsFallingHash, true);
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        Owner.MovementY += Owner.Gravity * Owner.PlayerControllerData.FallGravityMultiplier * Time.deltaTime;
        Owner.MovementY = Mathf.Max(Owner.MovementY, Owner.PlayerControllerData.FallSpeedMin);
        SubState?.Update();
        CheckChangeState();
    }

    public override void Exit()
    {
        SubState?.Exit();
        Owner.PlayerVisual.Animator.SetBool(Owner.PlayerVisual.IsFallingHash, false);
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
        if (Owner.CharacterController.isGrounded)
        {
            Owner.StateMachine.ChangeState(Owner.StateFactory.Grounded());
        }
        else if (Owner.IsJumpPressed && Owner.IsJumpBuffer && Owner.IsCoyoteTime)
        {
            //점프 버퍼와 코요테 타임을 모두 만족할 때 점프
            Owner.StopJumpBufferCoroutine();
            Owner.StopCoyoteTimeCoroutine();
            Owner.StateMachine.ChangeState(Owner.StateFactory.Jump());
        }
    }
}