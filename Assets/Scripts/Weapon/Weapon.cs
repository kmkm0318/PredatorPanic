/// <summary>
/// 무기 추상 클래스
/// 무기 데이터로 초기화되고 공격 기능을 정의
/// </summary>
public abstract class Weapon
{
    #region 데이터
    public WeaponData WeaponData { get; private set; }
    #endregion

    #region 레퍼런스
    public Player Player { get; private set; }
    #endregion

    /// <summary>
    /// 무기 생성자
    /// </summary>
    public Weapon(WeaponData weaponData)
    {
        WeaponData = weaponData;
    }

    /// <summary>
    /// 무기 장착 시 호출 메서드
    /// </summary>
    public virtual void OnEquip(Player player)
    {
        Player = player;
    }

    /// <summary>
    /// 무기 설명 반환 메서드
    /// 기본적으로 데이터의 설명을 그대로 반환합니다.
    /// 런타임에 동적으로 변경된 설명이 필요한 경우 오버라이드합니다.
    /// </summary>
    public virtual string GetDescription()
    {
        return WeaponData.GetDescription();
    }

    /// <summary>
    /// 무기 공격 처리 메서드
    /// </summary>
    public abstract void HandleAttack();
}