/// <summary>
/// 플레이어 상태 팩토리 클래스
/// 각 상태의 인스턴스를 생성하는 메서드 제공
/// </summary>
public class PlayerStateFactory
{
    private PlayerController _owner;

    public PlayerStateFactory(PlayerController owner)
    {
        _owner = owner;
    }

    public PlayerIdleState Idle()
    {
        return new PlayerIdleState(_owner);
    }

    public PlayerMoveState Move()
    {
        return new PlayerMoveState(_owner);
    }

    public PlayerGroundedState Grounded()
    {
        return new PlayerGroundedState(_owner);
    }

    public PlayerJumpState Jump()
    {
        return new PlayerJumpState(_owner);
    }

    public PlayerFallState Fall()
    {
        return new PlayerFallState(_owner);
    }
}