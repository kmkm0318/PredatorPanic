using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 원형 적 공격 패턴 데이터
/// _axis는 원형 평면의 법선 벡터
/// _axis가 0벡터일 경우 랜덤한 방향 설정
/// </summary>
[CreateAssetMenu(fileName = "CircleEnemyAttackPatternData", menuName = "SO/Enemy/EnemyAttackPattern/CircleEnemyAttackPatternData", order = 0)]
public class CircleEnemyAttackPatternData : EnemyAttackPatternData
{
    [Header("Circle Data")]
    [SerializeField] private Vector3 _axis = Vector3.zero;
    [SerializeField] private int _count = 8;
    [SerializeField] private float _radius = 4f;

    public override List<Vector3> GetAttackPositions(Enemy enemy, Player player)
    {
        // 타겟 위치 가져오기
        var targetPos = GetTargetPosition(enemy, player);

        // 공격 위치 리스트 생성
        List<Vector3> AttackPositions = new();

        // 개수가 0 이하일 경우 빈 리스트 반환
        if (_count <= 0) return AttackPositions;

        // 반지름이 0일 경우 타겟 위치 하나만 추가 후 반환
        if (_radius == 0)
        {
            AttackPositions.Add(targetPos);
            return AttackPositions;
        }

        // 축 정규화
        var axis = _axis.normalized;

        // 축이 0벡터일 경우 랜덤한 단위 벡터로 설정
        if (axis == Vector3.zero)
        {
            axis = Random.onUnitSphere;
        }

        // 오른쪽 벡터 구하기
        Vector3 right = Vector3.Cross(axis, Vector3.up).normalized;
        if (right == Vector3.zero)
        {
            // 오른쪽 벡터가 0벡터일 경우 right와 교차하여 구함
            right = Vector3.Cross(axis, Vector3.right).normalized;
        }

        // 위 벡터 구하기
        Vector3 up = Vector3.Cross(right, axis).normalized;

        // 지정된 개수만큼 공격 위치 계산
        for (int i = 0; i < _count; i++)
        {
            // 각도 계산
            float angle = i * Mathf.PI * 2 / _count;

            // x, z 좌표 계산
            float x = Mathf.Cos(angle) * _radius;
            float z = Mathf.Sin(angle) * _radius;

            // 축에 맞는 오프셋 계산 
            Vector3 offset = right * x + up * z;

            // 공격 위치 계산 및 추가
            Vector3 attackPos = targetPos + offset;
            AttackPositions.Add(attackPos);
        }

        // 반환
        return AttackPositions;
    }
}