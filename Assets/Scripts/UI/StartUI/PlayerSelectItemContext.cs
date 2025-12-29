/// <summary>
/// 플레이어 선택 아이템 컨텍스트
/// 플레이어 선택 아이템 UI에 필요한 데이터 컨텍스트
/// </summary>
public class PlayerSelectItemContext
{
    public PlayerData PlayerData;
    public bool IsUnlocked;
    public bool IsSelected;

    public PlayerSelectItemContext(PlayerData playerData, bool isUnlocked, bool isSelected)
    {
        PlayerData = playerData;
        IsUnlocked = isUnlocked;
        IsSelected = isSelected;
    }
}