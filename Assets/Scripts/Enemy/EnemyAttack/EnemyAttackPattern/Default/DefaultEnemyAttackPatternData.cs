using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본 적 공격 패턴 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "DefaultEnemyAttackPatternData", menuName = "SO/Enemy/EnemyAttackPattern/DefaultEnemyAttackPatternData", order = 0)]
public class DefaultEnemyAttackPatternData : EnemyAttackPatternData
{
    public override List<Vector3> GetAttackPositions(Enemy enemy, Player player)
    {
        // 타겟 위치 가져오기
        var targetPos = GetTargetPosition(enemy, player);

        // 공격 위치 리스트 생성
        List<Vector3> AttackPositions = new()
        {
            //타겟 위치 하나만 포함
            targetPos
        };

        // 반환
        return AttackPositions;
    }
}