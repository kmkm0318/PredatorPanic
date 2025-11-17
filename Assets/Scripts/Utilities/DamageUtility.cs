/// <summary>
/// 데미지 관련 유틸리티 클래스
/// </summary>
public static class DamageUtility
{
    //방어도 상수
    public const float DEFENSE_CONSTANT = 50f;

    /// <summary>
    /// 데미지 계산 메서드. 방어도를 고려하여 최종 데미지 반환
    /// </summary>
    public static float CalculateDamage(float damage, float defense)
    {
        float damageReduction = defense / (defense + DEFENSE_CONSTANT);
        return damage * (1f - damageReduction);
    }
}