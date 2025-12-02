using UnityEngine;

/// <summary>
/// 총알 발사 컨텍스트 구조체
/// </summary>
public struct BulletFireContext
{
    //발사한 총기
    public Gun Gun;
    //총알 발사 방향
    public Vector3 FireDirection;
    //총알 기본 데미지
    public float BaseDamage;
    //총알 속도
    public float Speed;
    //총알 사거리
    public float Range;
    //치명타 확률
    public float CriticalRate;
    //치명타 데미지 배율
    public float CriticalDamageRate;
    //관통 수
    public int PenetrationCount;
    //튕김 수
    public int RicochetCount;
    //충돌 레이어 마스크
    public LayerMask HitLayerMask;
}