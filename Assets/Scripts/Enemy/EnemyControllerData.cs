using UnityEngine;

/// <summary>
/// 적 컨트롤러 데이터 스크립터블 오브젝트 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyControllerData", menuName = "SO/Enemy/EnemyControllerData", order = 0)]
public class EnemyControllerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private EnemyMoveType _moveType;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _minMoveDistanceSqr = 0.25f;
    public EnemyMoveType MoveType => _moveType;
    public float RotationSpeed => _rotationSpeed;
    public float MinMoveDistanceSqr => _minMoveDistanceSqr;

    [Header("Ground")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayerMask;
    public float Gravity => _gravity;
    public float GroundCheckDistance => _groundCheckDistance;
    public LayerMask GroundLayerMask => _groundLayerMask;
}
