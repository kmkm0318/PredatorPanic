/// <summary>
/// 총 관련 유틸리티 클래스
/// </summary>
public static class GunUtility
{
    /// <summary>
    /// 총알이 적중했을 때 데미지를 계산하는 유틸리티 메서드
    /// </summary>
    public static float CalculateBulletDamage(Player player, Gun gun, float distanceTraveled)
    {
        float baseDamage = gun.GunStats.GetStat(GunStatType.Damage).FinalValue;
        float attackMultiplier = player.PlayerStats.GetStat(PlayerStatType.Attack).FinalValue;

        float range = gun.GunStats.GetStat(GunStatType.Range).FinalValue;
        float halfRange = range / 2f;
        float distanceFactor = 1.0f;
        if (distanceTraveled > halfRange)
        {
            distanceFactor = 1f - (distanceTraveled - halfRange) / halfRange * 0.5f; // 최대 50% 감소
        }

        return baseDamage * attackMultiplier * distanceFactor;
    }
}