/// <summary>
/// 플레이어가 적에게 데미지를 가했을 때의 정보
/// </summary>
public readonly struct PlayerDamageContext
{
    //공격자
    public Player Player { get; }
    //공격에 사용된 무기
    public Weapon Weapon { get; }
    //피해를 입은 적
    public Enemy Enemy { get; }
    //입힌 데미지
    public float Damage { get; }
    //치명타 여부
    public bool IsCritical { get; }

    public PlayerDamageContext(Player player, Weapon weapon, Enemy enemy, float damage, bool isCritical)
    {
        Player = player;
        Weapon = weapon;
        Enemy = enemy;
        Damage = damage;
        IsCritical = isCritical;
    }
}