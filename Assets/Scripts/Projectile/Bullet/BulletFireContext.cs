using UnityEngine;

/// <summary>
/// 총알 발사 컨텍스트 구조체
/// </summary>
public readonly struct BulletFireContext
{
    //발사한 플레이어
    public readonly Player Player;
    //첫 목표 적
    public readonly Enemy InitialTargetEnemy;
    //발사한 총기
    public readonly Gun Gun;
    //총알 궤적 이펙트
    public readonly Trail Trail;
    //발사한 위치
    public readonly Vector3 FirePosition;
    //총알 발사 방향
    public readonly Vector3 FireDirection;
    //총알 기본 데미지
    public readonly float BaseDamage;
    //총알 속도
    public readonly float Speed;
    //총알 사거리
    public readonly float Range;
    //치명타 확률
    public readonly float CriticalRate;
    //치명타 데미지 배율
    public readonly float CriticalDamageRate;
    //관통 수
    public readonly int PenetrationCount;
    //튕김 수
    public readonly int RicochetCount;
    //폭발 데이터
    public readonly ExplosionData ExplosionData;
    //충돌 레이어 마스크
    public readonly LayerMask HitLayerMask;

    public BulletFireContext(Player player, Enemy initialTargetEnemy, Gun gun, Trail trail, Vector3 firePosition, Vector3 fireDirection, float baseDamage, float speed, float range, float criticalRate, float criticalDamageRate, int penetrationCount, int ricochetCount, ExplosionData explosionData, LayerMask hitLayerMask)
    {
        Player = player;
        InitialTargetEnemy = initialTargetEnemy;
        Gun = gun;
        Trail = trail;
        FirePosition = firePosition;
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