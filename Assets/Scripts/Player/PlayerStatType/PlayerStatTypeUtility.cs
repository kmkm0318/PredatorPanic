using System.Collections.Generic;

/// <summary>
/// 플레이어 스탯 타입 확장 클래스
/// </summary>
public static class PlayerStatTypeUtility
{
    /// <summary>
    /// 스탯의 기본값 딕셔너리
    /// </summary>
    private static readonly Dictionary<PlayerStatType, float> _defaultValues = new()
    {
        { PlayerStatType.Health, 50f },
        { PlayerStatType.Attack, 50f },
        { PlayerStatType.Defense, 50f },
        { PlayerStatType.MoveSpeed, 5f },
        { PlayerStatType.AttackSpeed, 1f },
        { PlayerStatType.CriticalRate, 0f },
        { PlayerStatType.CriticalDamageRate, 1.5f },
        { PlayerStatType.MagnetRadius, 5f },
        { PlayerStatType.EXPGainRate, 1f },
        { PlayerStatType.ToothGainRate, 1f },
        { PlayerStatType.DNAGainRate, 1f },
        { PlayerStatType.BulletCount, 1f },
        { PlayerStatType.PenetrationCount, 0f },
        { PlayerStatType.RicochetCount, 0f },
        { PlayerStatType.LifeSteal, 0f },
        { PlayerStatType.JumpForce, 5f },
        { PlayerStatType.AirJumpCount, 0f },
        { PlayerStatType.InvincibleDuration, 0.5f },
        { PlayerStatType.Luck, 0f },
    };

    /// <summary>
    /// 스탯의 기본값 반환
    /// </summary>
    public static float GetDefaultValue(this PlayerStatType statType)
    {
        if (_defaultValues.TryGetValue(statType, out var value))
        {
            return value;
        }
        else
        {
            $"Default value not defined for PlayerStatType: {statType}".LogError();
            return 0f;
        }
    }
}