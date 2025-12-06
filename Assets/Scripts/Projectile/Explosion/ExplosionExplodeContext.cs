using UnityEngine;

/// <summary>
/// 폭발 컨텍스트 구조체
/// </summary>
public struct ExplosionExplodeContext
{
    //폭발을 일으킨 플레이어
    public Player Player;
    //폭발을 일으킨 무기
    public Weapon Weapon;
    //폭발 기본 데미지
    public float BaseDamage;
    //폭발 반경
    public float Radius;
    //치명타 확률
    public float CriticalRate;
    //치명타 데미지 배율
    public float CriticalDamageRate;
    //목표 레이어 마스크
    public LayerMask HitLayerMask;
}