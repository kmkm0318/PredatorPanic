/// <summary>
/// 플레이어가 적에게 데미지를 가했을 때의 정보
/// </summary>
public struct PlayerDamageContext
{
    //공격자
    public Player Player;
    //공격에 사용된 무기
    public Weapon Weapon;
    //피해를 입은 적
    public Enemy Enemy;
    //입힌 데미지 양
    public float Damage;
    //치명타 여부
    public bool IsCritical;
}