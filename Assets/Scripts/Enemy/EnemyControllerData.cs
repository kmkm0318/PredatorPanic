using UnityEngine;

/// <summary>
/// 적 컨트롤러 데이터 스크립터블 오브젝트 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyControllerData", menuName = "SO/Enemy/EnemyControllerData", order = 0)]
public class EnemyControllerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private EnemyMoveType _moveType;
    [SerializeField] private float _minMoveDistanceSqr = 1f;
    public EnemyMoveType MoveType => _moveType;
    public float MinMoveDistanceSqr => _minMoveDistanceSqr;
}
