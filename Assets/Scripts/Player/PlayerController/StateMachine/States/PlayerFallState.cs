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
        //떨어지는 애니메이션 재생
        PlayerController.PlayerVisual.Animator.SetBool(PlayerController.PlayerVisual.IsFallingHash, true);

        //서브 상태 초기화
        InitSubState();
        SubState?.Enter();
    }

    public override void Update()
    {
        //중력 적용
        PlayerController.MovementY += PlayerController.Gravity * PlayerController.PlayerControllerData.FallGravityMultiplier * Time.deltaTime;

        //최소 낙하 속도 제한
        PlayerController.MovementY = Mathf.Max(PlayerController.MovementY, PlayerController.PlayerControllerData.FallSpeedMin);

        //서브 상태 업데이트
        SubState?.Update();

        //상태 전환 체크
        CheckChangeState();
    }

    public override void Exit()
    {
        //서브 상태 종료
        SubState?.Exit();

        //떨어지는 애니메이션 종료
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
        if (PlayerController.IsGrounded)
        {
            PlayerController.StateMachine.ChangeState(Factory.Grounded);
        }
        else if (PlayerController.IsJumpPressed && PlayerController.IsJumpBuffer && PlayerController.IsCoyoteTime)
        {
            //점프 버퍼와 코요테 타임을 모두 만족할 때 점프.
            //이때는 공중 점프 횟수를 소모하지 않음.
            PlayerController.StopJumpBufferCoroutine();
            PlayerController.StopCoyoteTimeCoroutine();
            PlayerController.StateMachine.ChangeState(Factory.Jump);
        }
        else if (PlayerController.IsJumpPressed && PlayerController.IsJumpBuffer)
        {
            //공중 점프 시도
            if (PlayerController.TryAirJump())
            {
                PlayerController.StopJumpBufferCoroutine();
                PlayerController.StateMachine.ChangeState(Factory.Jump);
            }
        }
    }
}