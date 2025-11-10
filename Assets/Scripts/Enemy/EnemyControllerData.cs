using UnityEngine;

[CreateAssetMenu(fileName = "EnemyControllerData", menuName = "SO/Enemy/EnemyControllerData", order = 0)]
public class EnemyControllerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _groundedGravitySpeed = -0.5f;
    [SerializeField] private float _gravity = -9.8f;
    public float MoveSpeed => _moveSpeed;
    public float GroundedGravitySpeed => _groundedGravitySpeed;
    public float Gravity => _gravity;
}