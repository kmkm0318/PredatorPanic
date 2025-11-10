using UnityEngine;

/// <summary>
/// 플레이어 컨트롤러 데이터 스크립터블 오브젝트
/// 플레이어 이동, 점프, 회전에 관한 설정 값 보유
/// </summary>
[CreateAssetMenu(fileName = "PlayerControllerData", menuName = "SO/Player/PlayerControllerData", order = 0)]
public class PlayerControllerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _groundedGravitySpeed = -0.5f;
    [SerializeField] private float _fallGravityMultiplier = 2f;
    [SerializeField] private float _fallSpeedMin = -20f;
    public float MoveSpeed => _moveSpeed;
    public float GroundedGravitySpeed => _groundedGravitySpeed;
    public float FallGravityMultiplier => _fallGravityMultiplier;
    public float FallSpeedMin => _fallSpeedMin;

    [Header("Rotation")]
    [SerializeField] private float _mouseSensitivity = 10f;
    [SerializeField] private float _controllerSensitivity = 500f;
    [SerializeField] private float _pitchMin = -90f;
    [SerializeField] private float _pitchMax = 90f;
    public float MouseSensitivity => _mouseSensitivity;
    public float ControllerRotateSpeed => _controllerSensitivity;
    public float PitchMin => _pitchMin;
    public float PitchMax => _pitchMax;

    [Header("Jump")]
    [SerializeField] private float _maxJumpHeight = 2.5f;
    [SerializeField] private float _maxJumpDuration = 1f;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private float _coyoteTime = 0.2f;
    public float MaxJumpHeight => _maxJumpHeight;
    public float MaxJumpDuration => _maxJumpDuration;
    public float JumpBufferTime => _jumpBufferTime;
    public float CoyoteTime => _coyoteTime;
}