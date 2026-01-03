using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 물리 관련 유틸리티 클래스
/// 콜라이더 배열 버퍼를 재사용하여 할당을 최소화합니다.
/// </summary>
public static class PhysicsUtility
{
    #region 상수
    //콜라이더 버퍼 최대 크기
    private const int MAX_COLLIDER_BUFFER_SIZE = 128;

    //레이캐스트 히트 버퍼 최대 크기
    private const int MAX_RAYCAST_HIT_BUFFER_SIZE = 16;
    #endregion

    #region 버퍼
    //콜라이더 배열 재사용을 위한 버퍼
    private static Collider[] _colliders = new Collider[MAX_COLLIDER_BUFFER_SIZE];

    //레이캐스트 히트 배열 재사용을 위한 버퍼
    private static RaycastHit[] _raycastHits = new RaycastHit[MAX_RAYCAST_HIT_BUFFER_SIZE];
    #endregion

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
    public static int GetOverlapSphereNonAlloc(Vector3 position, float radius, LayerMask layerMask, out Collider[] colliders, QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.UseGlobal)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(position, radius, _colliders, layerMask, queryTrigger);
        colliders = _colliders;
        return hitCount;
    }

    /// <summary>
    /// RaycastNonAlloc 래퍼 함수
    /// RaycastHit 배열 버퍼를 재사용하여 할당 최소화
    /// </summary>
    public static int RaycastNonAlloc(Vector3 position, Vector3 direction, float distance, LayerMask layerMask, out RaycastHit[] hitInfo, QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.UseGlobal)
    {
        Ray ray = new(position, direction);
        int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, distance, layerMask, queryTrigger);
        hitInfo = _raycastHits;
        return hitCount;
    }
}