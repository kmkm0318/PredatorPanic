/// <summary>
/// 무기 선택 아이템 컨텍스트
/// 무기 선택 아이템 UI에 필요한 데이터 컨텍스트
/// </summary>
public class WeaponSelectItemContext
{
    public WeaponData WeaponData;
    public bool IsUnlocked;
    public bool IsSelected;

    public WeaponSelectItemContext(WeaponData weaponData, bool isUnlocked, bool isSelected)
    {
        WeaponData = weaponData;
        IsUnlocked = isUnlocked;
        IsSelected = isSelected;
    }
}