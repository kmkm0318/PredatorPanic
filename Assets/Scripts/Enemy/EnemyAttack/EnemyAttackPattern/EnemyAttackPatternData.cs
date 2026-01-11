using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격 패턴 데이터 추상 클래스
/// </summary>
public abstract class EnemyAttackPatternData : ScriptableObject
{
    [Header("Enemy Attack Pattern Data")]
    [SerializeField] private IndicatedAttackData _indicatedAttackData;
    [SerializeField] private EnemyAttackPatternTargetType _targetType;
    [SerializeField] private float _attackRadius = 2f;
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private float _minAttackCooldown = 1f;
    [SerializeField] private float _maxAttackCooldown = 2f;
    public IndicatedAttackData IndicatedAttackData => _indicatedAttackData;
    public EnemyAttackPatternTargetType TargetType => _targetType;
    public float AttackRadius => _attackRadius;
    public float AttackDelay => _attackDelay;
    public float AttackCooldown => Random.Range(_minAttackCooldown, _maxAttackCooldown);

    /// <summary>
    /// 플레이어의 위치를 기반으로 공격 위치 리스트를 반환합니다.
    /// IsPredict를 통해 예측 공격 여부를 결정합니다.
    /// </summary>
    public abstract List<Vector3> GetAttackPositions(Enemy enemy, Player player);
}
