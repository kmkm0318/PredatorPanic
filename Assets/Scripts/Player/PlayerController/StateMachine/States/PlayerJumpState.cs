using UnityEngine;

/// <summary>
/// 플레이어가 점프하는 상태
/// 지상에서 점프 키를 눌렀을 때 진입
/// </summary>
public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController owner) : base(owner) { }

    public override void Enter()
    {
        Owner.PlayerVisual.Animator.SetBool(Owner.PlayerVisual.IsJumpingHash, true);
        Owner.MovementY = Owner.InitialJumpSpeed;
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        Owner.MovementY += Owner.Gravity * Time.deltaTime;
        SubState?.Update();
        CheckChangeState();
    }

    public override void Exit()
    {
        SubState?.Exit();
        Owner.PlayerVisual.Animator.SetBool(Owner.PlayerVisual.IsJumpingHash, false);
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
        else if (Owner.MovementY <= 0 || !Owner.IsJumpPressed)
        {
            Owner.StateMachine.ChangeState(Owner.StateFactory.Fall());
        }
    }
}