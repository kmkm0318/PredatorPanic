/// <summary>
/// 플레이어 선택 아이템 컨텍스트
/// 플레이어 선택 아이템 UI에 필요한 데이터 컨텍스트
/// </summary>
public class RunSelectItemContext
{
    public RunData RunData;
    public bool IsUnlocked;
    public bool IsSelected;

    public RunSelectItemContext(RunData runData, bool isUnlocked, bool isSelected)
    {
        RunData = runData;
        IsUnlocked = isUnlocked;
        IsSelected = isSelected;
    }
}