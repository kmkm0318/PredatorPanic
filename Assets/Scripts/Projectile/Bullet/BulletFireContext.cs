using UnityEngine;

/// <summary>
/// 총알 발사 컨텍스트 구조체
/// </summary>
public readonly struct BulletFireContext
{
    //발사한 플레이어
    public Player Player { get; }
    //발사한 총기
    public Gun Gun { get; }
    //총알 발사 방향
    public Vector3 FireDirection { get; }
    //총알 기본 데미지
    public float BaseDamage { get; }
    //총알 속도
    public float Speed { get; }
    //총알 사거리
    public float Range { get; }
    //치명타 확률
    public float CriticalRate { get; }
    //치명타 데미지 배율
    public float CriticalDamageRate { get; }
    //관통 수
    public int PenetrationCount { get; }
    //튕김 수
    public int RicochetCount { get; }
    //폭발 데이터
    public ExplosionData ExplosionData { get; }
    //충돌 레이어 마스크
    public LayerMask HitLayerMask { get; }

    public BulletFireContext(Player player, Gun gun, Vector3 fireDirection, float baseDamage, float speed, float range, float criticalRate, float criticalDamageRate, int penetrationCount, int ricochetCount, ExplosionData explosionData, LayerMask hitLayerMask)
    {
        Player = player;
        Gun = gun;
        FireDirection = fireDirection;
        BaseDamage = baseDamage;
        Speed = speed;
        Range = range;
        CriticalRate = criticalRate;
        CriticalDamageRate = criticalDamageRate;
        PenetrationCount = penetrationCount;
        RicochetCount = ricochetCount;
        ExplosionData = explosionData;
        HitLayerMask = hitLayerMask;
    }
}