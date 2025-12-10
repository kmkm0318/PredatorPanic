using UnityEngine;

/// <summary>
/// 폭발 컨텍스트 구조체
/// </summary>
public readonly struct ExplosionExplodeContext
{
    //폭발을 일으킨 플레이어
    public Player Player { get; }
    //폭발을 일으킨 무기
    public Weapon Weapon { get; }
    //폭발 기본 데미지
    public float BaseDamage { get; }
    //폭발 반경
    public float Radius { get; }
    //치명타 확률
    public float CriticalRate { get; }
    //치명타 데미지 배율
    public float CriticalDamageRate { get; }
    //목표 레이어 마스크
    public LayerMask HitLayerMask { get; }

    public ExplosionExplodeContext(Player player, Weapon weapon, float baseDamage, float radius, float criticalRate, float criticalDamageRate, LayerMask hitLayerMask)
    {
        Player = player;
        Weapon = weapon;
        BaseDamage = baseDamage;
        Radius = radius;
        CriticalRate = criticalRate;
        CriticalDamageRate = criticalDamageRate;
        HitLayerMask = hitLayerMask;
    }
}