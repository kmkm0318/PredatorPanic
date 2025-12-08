using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 물리 관련 유틸리티 클래스
/// 콜라이더 배열 버퍼를 재사용하여 할당을 최소화합니다.
/// </summary>
public static class PhysicsUtility
{
    //콜라이더 버퍼 최대 크기
    private const int MAX_COLLIDER_BUFFER_SIZE = 128;
    //콜라이더 배열 재사용을 위한 버퍼
    private static Collider[] _colliders = new Collider[MAX_COLLIDER_BUFFER_SIZE];

    /// <summary>
    /// 가장 가까운 타겟 찾기
    /// </summary>
    public static Collider GetNearestCollider(Vector3 origin, float range, LayerMask targetLayerMask, HashSet<Collider> excepts = null)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(origin, range, _colliders, targetLayerMask);

        Collider nearestCollider = null;
        float minDistSqr = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            var collider = _colliders[i];

            if (excepts != null && excepts.Contains(collider)) continue;

            float curDistSqr = (collider.transform.position - origin).sqrMagnitude;
            if (curDistSqr < minDistSqr)
            {
                minDistSqr = curDistSqr;
                nearestCollider = collider;
            }
        }

        return nearestCollider;
    }

    /// <summary>
    /// OverlapSphereNonAlloc 래퍼 함수
    /// 콜라이더 배열 버퍼를 재사용하여 할당 최소화
    /// </summary>
    public static int GetOverlapSphereNonAlloc(Vector3 position, float radius, LayerMask layerMask, out Collider[] colliders)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(position, radius, _colliders, layerMask);
        colliders = _colliders;
        return hitCount;
    }
}