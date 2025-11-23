/// <summary>
/// 플레이어 상태 팩토리 클래스
/// 플레이어 컨트롤러의 상태를 캐싱해둔 후 프로퍼티로 제공
/// </summary>
public class PlayerStateFactory
{
    private PlayerController _player;

    public PlayerIdleState Idle { get; }
    public PlayerMoveState Move { get; }
    public PlayerGroundedState Grounded { get; }
    public PlayerJumpState Jump { get; }
    public PlayerFallState Fall { get; }

    public PlayerStateFactory(PlayerController player)
    {
        _player = player;

        Idle = new(_player, this);
        Move = new(_player, this);
        Grounded = new(_player, this);
        Jump = new(_player, this);
        Fall = new(_player, this);
    }
}