/// <summary>
/// 총기 스탯 타입
/// </summary>
public enum GunStatType
{
    Damage, //기본 총기 데미지
    FireSpeed, //발사 속도. 1초에 몇 번 발사하는지
    BulletSpeed, //총알 속도
    Range, //사거리. 사거리 절반 이내까지 최대 데미지
    CriticalRate, //치명타 확률
    CriticalDamageRate, //치명타 데미지 배율
    BulletCount, //한 발에 발사되는 총알 수
    PenetrationCount, //관통 가능한 적 수
    RicochetCount, //튕겨나가는 횟수
}