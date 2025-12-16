using UnityEngine;

/// <summary>
/// 폭발 컨텍스트 구조체
/// </summary>
public readonly struct ExplosionExplodeContext
{
    //폭발을 일으킨 플레이어
    public readonly Player Player;
    //폭발을 일으킨 무기
    public readonly Weapon Weapon;
    //폭발 위치
    public readonly Vector3 ExplodePosition;
    //폭발 기본 데미지
    public readonly float BaseDamage;
    //폭발 반경
    public readonly float Radius;
    //치명타 확률
    public readonly float CriticalRate;
    //치명타 데미지 배율
    public readonly float CriticalDamageRate;
    //목표 레이어 마스크
    public readonly LayerMask HitLayerMask;

    public ExplosionExplodeContext(Player player, Weapon weapon, Vector3 explodePosition, float baseDamage, float radius, float criticalRate, float criticalDamageRate, LayerMask hitLayerMask)
    {
        Player = player;
        Weapon = weapon;
        ExplodePosition = explodePosition;
        BaseDamage = baseDamage;
        Radius = radius;
        CriticalRate = criticalRate;
        CriticalDamageRate = criticalDamageRate;
        HitLayerMask = hitLayerMask;
    }
}