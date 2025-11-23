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
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsJumpingHash, true);
        PlayerController.MovementY = PlayerController.InitialJumpSpeed;
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        PlayerController.MovementY += PlayerController.Gravity * Time.deltaTime;
        SubState?.Update();
        CheckChangeState();
    }

    public override void Exit()
    {
        SubState?.Exit();
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsJumpingHash, false);
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
        else if (PlayerController.MovementY <= 0 || !PlayerController.IsJumpPressed)
        {
            PlayerController.StateMachine.ChangeState(Factory.Fall);
        }
    }
}