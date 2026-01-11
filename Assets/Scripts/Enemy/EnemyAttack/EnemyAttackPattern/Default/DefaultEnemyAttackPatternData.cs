using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본 적 공격 패턴 데이터 클래스
/// 플레이어 위치, 예측 위치, 적 위치를 기반으로 공격 위치 반환
/// </summary>
[CreateAssetMenu(fileName = "DefaultEnemyAttackPatternData", menuName = "SO/Enemy/EnemyAttackPattern/DefaultEnemyAttackPatternData", order = 0)]
public class DefaultEnemyAttackPatternData : EnemyAttackPatternData
{
    public override List<Vector3> GetAttackPositions(Enemy enemy, Player player)
    {
        var targetPos = TargetType switch
        {
            EnemyAttackPatternTargetType.PlayerPosition => player.CenterPosition,
            EnemyAttackPatternTargetType.PlayerPredictedPosition => player.GetPredictedPosition(AttackDelay),
            EnemyAttackPatternTargetType.EnemyPosition => enemy.CenterPosition,
            _ => player.CenterPosition,
        };

        List<Vector3> AttackPositions = new()
        {
            targetPos
        };

        return AttackPositions;
    }
}