using UnityEngine;

/// <summary>
/// 적 컨트롤러 데이터 스크립터블 오브젝트 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyControllerData", menuName = "SO/Enemy/EnemyControllerData", order = 0)]
public class EnemyControllerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _groundedGravitySpeed = -0.5f;
    [SerializeField] private float _gravity = -9.8f;
    public float GroundedGravitySpeed => _groundedGravitySpeed;
    public float Gravity => _gravity;
}