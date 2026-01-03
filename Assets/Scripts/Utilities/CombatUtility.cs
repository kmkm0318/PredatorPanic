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
    //총기 Range에 대한 폭발 반경 비율
    public const float EXPLOSION_RADIUS_RATIO = 0.1f;

    /// <summary>
    /// 공격력에 따른 데미지 계산 함수
    /// </summary>
    public static float CalculateAttackDamage(float baseDamage, float attack)
    {
        float attackMultiplier = Mathf.Max(attack, 0f) / ATTACK_CONSTANT;
        return baseDamage * attackMultiplier;
    }

    /// <summary>
    /// 플레이어의 공격력과 총기의 기본 데미지를 통한 총알의 기본 데미지를 반환하는 함수
    /// </summary>
    public static float CalculateBulletBaseDamage(Player player, Gun gun)
    {
        float baseDamage = gun.GunStats.GetStat(GunStatType.Damage).FinalValue;
        float attack = player.PlayerStats.GetStat(PlayerStatType.Attack).FinalValue;

        return CalculateAttackDamage(baseDamage, attack);
    }

    /// <summary>
    /// 데미지를 받았을 때 방어력을 통해 줄어든 데미지를 얻는 계산 함수
    /// 방어도를 고려하여 최종 데미지 반환
    /// 플레이어가 공격을 당할 경우에만 사용
    /// 적은 방어력 계산을 하지 않음
    /// </summary>
    public static float CalculateDefensedDamage(float damage, float defense)
    {
        float damageMultiplier = DEFENSE_CONSTANT / Mathf.Max(defense, 1f);
        return damage * damageMultiplier;
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
    /// 치명타 확률 계산 함수
    /// </summary>
    public static float CalculateCriticalRate(Player player, Gun gun)
    {
        float baseCriticalRate = gun.GunStats.GetStat(GunStatType.CriticalRate).FinalValue;
        float criticalRateBonus = player.PlayerStats.GetStat(PlayerStatType.CriticalRate).FinalValue;

        return baseCriticalRate + criticalRateBonus;
    }

    /// <summary>
    /// 치명타 데미지 계산 함수
    /// 치명타 데미지 배율을 적용하여 최종 치명타 데미지를 반환
    /// </summary>
    public static float CalculateCriticalDamageRate(Player player, Gun gun)
    {
        float gunCriticalDamageRate = gun.GunStats.GetStat(GunStatType.CriticalDamageRate).FinalValue;
        float playerCriticalDamageRate = player.PlayerStats.GetStat(PlayerStatType.CriticalDamageRate).FinalValue;

        float totalCriticalDamageRate = gunCriticalDamageRate * playerCriticalDamageRate;

        return totalCriticalDamageRate;
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
    /// 탄환 속도 계산 함수
    /// </summary>
    public static float CalculateBulletSpeed(Player player, Gun gun)
    {
        float baseBulletSpeed = gun.GunStats.GetStat(GunStatType.BulletSpeed).FinalValue;
        float additionalBulletSpeed = player.PlayerStats.GetStat(PlayerStatType.BulletSpeed).FinalValue;

        return baseBulletSpeed * additionalBulletSpeed;
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