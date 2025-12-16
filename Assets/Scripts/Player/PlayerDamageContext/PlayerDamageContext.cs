/// <summary>
/// 플레이어가 적에게 데미지를 가했을 때의 정보
/// </summary>
public readonly struct PlayerDamageContext
{
    //공격자
    public readonly Player Player;
    //공격에 사용된 무기
    public readonly Weapon Weapon;
    //피해를 입은 적
    public readonly Enemy Enemy;
    //입힌 데미지
    public readonly float Damage;
    //치명타 여부
    public readonly bool IsCritical;
    //데미지 출처 타입
    public readonly PlayerDamageSourceType DamageSourceType;

    public PlayerDamageContext(Player player, Weapon weapon, Enemy enemy, float damage, bool isCritical, PlayerDamageSourceType damageSourceType)
    {
        Player = player;
        Weapon = weapon;
        Enemy = enemy;
        Damage = damage;
        IsCritical = isCritical;
        DamageSourceType = damageSourceType;
    }
}
