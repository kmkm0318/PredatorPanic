using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데미지 관련 유틸리티 클래스
/// </summary>
public static class CombatUtility
{
    //공격력 상수
    public const float ATTACK_CONSTANT = 50f;
    //방어도 상수
    public const float DEFENSE_CONSTANT = 50f;

    /// <summary>
    /// 총알이 적중했을 때 데미지를 계산하는 함수
    /// 공격력을 고려하여 최종 데미지 반환
    /// </summary>
    public static float CalculateBulletDamage(Player player, Gun gun, float distanceTraveled)
    {
        float baseDamage = gun.GunStats.GetStat(GunStatType.Damage).FinalValue;
        float attack = player.PlayerStats.GetStat(PlayerStatType.Attack).FinalValue;
        float attackMultiplier = attack / (attack + ATTACK_CONSTANT);

        float range = gun.GunStats.GetStat(GunStatType.Range).FinalValue;
        float halfRange = range / 2f;
        float distanceFactor = 1.0f;
        if (distanceTraveled > halfRange)
        {
            distanceFactor = 1f - (distanceTraveled - halfRange) / halfRange * 0.5f; // 최대 50% 감소
        }

        return baseDamage * attackMultiplier * distanceFactor;
    }

    /// <summary>
    /// 데미지를 받았을 때 방어력을 통해 줄어든 데미지를 얻는 계산 함수
    /// 방어도를 고려하여 최종 데미지 반환
    /// </summary>
    public static float CalculateDefensedDamage(float damage, float defense)
    {
        float damageReduction = defense / (defense + DEFENSE_CONSTANT);
        return damage * (1f - damageReduction);
    }

    /// <summary>
    /// 치명타 여부 계산 함수
    /// 플레이어와 총기의 치명타 확률을 합산하여 랜덤으로 치명타 여부 결정
    /// </summary>
    public static bool IsCritical(Player player, Gun gun)
    {
        float baseCriticalRate = gun.GunStats.GetStat(GunStatType.CriticalRate).FinalValue;
        float criticalRateBonus = player.PlayerStats.GetStat(PlayerStatType.CriticalRate).FinalValue;

        float totalCriticalRate = baseCriticalRate + criticalRateBonus;

        float rand = Random.Range(0f, 100f);
        return rand < totalCriticalRate;
    }

    /// <summary>
    /// 치명타 데미지 계산 함수
    /// 치명타 데미지 배율을 적용하여 최종 치명타 데미지를 반환
    /// </summary>
    public static float CalculateCriticalDamage(Player player, Gun gun, float damage)
    {
        float gunCriticalDamageRate = gun.GunStats.GetStat(GunStatType.CriticalDamageRate).FinalValue;
        float playerCriticalDamageRate = player.PlayerStats.GetStat(PlayerStatType.CriticalDamageRate).FinalValue;

        float totalCriticalDamageRate = gunCriticalDamageRate * playerCriticalDamageRate;

        return damage * (1f + totalCriticalDamageRate);
    }

    /// <summary>
    /// 공격 속도 계산 함수
    /// 플레이어와 총기의 공격 속도를 곱하여 최종 공격 속도 반환
    /// </summary>
    public static float CalculateFireSpeed(Player player, Gun gun)
    {
        float gunFireSpeed = gun.GunStats.GetStat(GunStatType.FireSpeed).FinalValue;
        float playerAttackSpeed = player.PlayerStats.GetStat(PlayerStatType.AttackSpeed).FinalValue;

        return gunFireSpeed * playerAttackSpeed;
    }

    /// <summary>
    /// 총알 수 계산 함수
    /// </summary>
    public static int CalculateBulletCount(Player player, Gun gun)
    {
        float baseBulletCount = gun.GunStats.GetStat(GunStatType.BulletCount).FinalValue;
        float additionalBulletCount = player.PlayerStats.GetStat(PlayerStatType.BulletCount).FinalValue;

        return (int)(baseBulletCount + additionalBulletCount);
    }

    /// <summary>
    /// 관통 수 계산 함수
    /// </summary>
    public static int CalculatePenetrationCount(Player player, Gun gun)
    {
        float basePenetrationCount = gun.GunStats.GetStat(GunStatType.PenetrationCount).FinalValue;
        float additionalPenetrationCount = player.PlayerStats.GetStat(PlayerStatType.PenetrationCount).FinalValue;

        return (int)(basePenetrationCount + additionalPenetrationCount);
    }

    /// <summary>
    /// 튕김 수 계산 함수
    /// </summary>
    public static int CalculateRicochetCount(Player player, Gun gun)
    {
        float baseRicochetCount = gun.GunStats.GetStat(GunStatType.RicochetCount).FinalValue;
        float additionalRicochetCount = player.PlayerStats.GetStat(PlayerStatType.RicochetCount).FinalValue;

        return (int)(baseRicochetCount + additionalRicochetCount);
    }

    /// <summary>
    /// 가장 가까운 타겟 찾기
    /// </summary>
    public static Collider GetNearestCollider(Vector3 origin, float range, LayerMask targetLayerMask, HashSet<Collider> excepts = null)
    {
        Collider[] hitColliders = Physics.OverlapSphere(origin, range, targetLayerMask);

        Collider nearestCollider = null;
        float minDistSqr = float.MaxValue;

        foreach (var collider in hitColliders)
        {
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
    /// 총알을 여러 개 발사할 때를 위해 퍼짐 각도 계산 함수
    /// </summary>
    /// <param name="fireDirection">기본 발사 방향</param>
    /// <param name="idx">현재 총알 인덱스</param>
    /// <param name="totalCount">총 발사할 총알 수</param>
    /// <param name="spreadAngle">각 총알 방향 사이 각도 (기본값 1도)</param>
    public static Vector3 GetSpreadDirection(Vector3 fireDirection, int idx, int totalCount, float spreadAngle = 1f)
    {
        //총알이 1개일 때는 퍼짐 없음
        if (totalCount == 1) return fireDirection;

        //퍼짐 각도 계산

        //가장 왼쪽 각도부터 시작
        float startAngle = -spreadAngle * (totalCount - 1) / 2f;
        float angle = startAngle + spreadAngle * idx;

        Quaternion spreadRotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 spreadDirection = spreadRotation * fireDirection;
        return spreadDirection.normalized;
    }
}