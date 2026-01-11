using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본 적 공격 패턴 데이터 클래스
/// 적 위치 혹은 예상 위치에 하나 소환
/// </summary>
[CreateAssetMenu(fileName = "DefaultEnemyAttackPatternData", menuName = "SO/Enemy/EnemyAttackPattern/DefaultEnemyAttackPatternData", order = 0)]
public class DefaultEnemyAttackPatternData : EnemyAttackPatternData
{
    public override List<Vector3> GetAttackPositions(Enemy enemy, Player player)
    {
        var targetPos = IsPredict ? player.GetPredictedPosition(AttackDelay) : player.CenterPosition;

        List<Vector3> AttackPositions = new()
        {
            targetPos
        };

        return AttackPositions;
    }
}