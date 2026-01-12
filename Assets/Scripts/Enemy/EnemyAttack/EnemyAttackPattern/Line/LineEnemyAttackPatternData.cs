using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 라인 적 공격 패턴 데이터 클래스
/// 타겟 위치를 중앙으로 하는 Direction 방향의 직선을 공격함
/// _direction이 0벡터일 경우 랜덤 방향으로 설정
/// </summary>
[CreateAssetMenu(fileName = "LineEnemyAttackPatternData", menuName = "SO/Enemy/EnemyAttackPattern/LineEnemyAttackPatternData", order = 0)]
public class LineEnemyAttackPatternData : EnemyAttackPatternData
{
    [Header("Line Data")]
    [SerializeField] private Vector3 _direction = Vector3.zero;
    [SerializeField] private int _count = 5;
    [SerializeField] private float _spacing = 4f;

    public override List<Vector3> GetAttackPositions(Enemy enemy, Player player)
    {
        // 타겟 위치 가져오기
        var targetPos = GetTargetPosition(enemy, player);

        // 공격 위치 리스트 생성
        List<Vector3> AttackPositions = new();

        // 개수가 0 이하일 경우 빈 리스트 반환
        if (_count <= 0) return AttackPositions;

        // 개수가 1이면 타겟 위치 하나만 추가 후 반환
        if (_count == 1)
        {
            AttackPositions.Add(targetPos);
            return AttackPositions;
        }

        // 방향 정규화
        var direction = _direction.normalized;

        if (direction == Vector3.zero)
        {
            // 방향이 0벡터일 경우 랜덤 값 설정
            direction = Random.onUnitSphere;
        }

        // 시작 위치 계산 (중앙 정렬)
        var startPos = targetPos - _spacing * (_count - 1) * direction / 2;

        // 지정된 개수만큼 공격 위치 계산
        for (int i = 0; i < _count; i++)
        {
            var offset = _spacing * i * direction;
            AttackPositions.Add(startPos + offset);
        }

        // 반환
        return AttackPositions;
    }
}