/// <summary>
/// 문자열 유틸리티 클래스
/// </summary>
public static class StringUtility
{
    /// <summary>
    /// 스탯 모디파이어의 설명을 반환합니다.
    /// 긍정적인 효과는 초록색, 부정적인 효과는 빨간색으로 표시됩니다.
    /// </summary>
    public static string GetModifierDescription(StatModifierType type, float value)
    {
        string color = string.Empty;
        string valueStr = string.Empty;

        switch (type)
        {
            case StatModifierType.Flat:
                color = value >= 0 ? "green" : "red";
                valueStr = $"{(value >= 0 ? "+" : "")}{value}";
                break;
            case StatModifierType.PercentAdd:
                color = value >= 0 ? "green" : "red";
                valueStr = $"{(value >= 0 ? "+" : "")}{value * 100}%";
                break;
            case StatModifierType.PercentMult:
                color = value >= 1 ? "green" : "red";
                valueStr = $"{value * 100}%";
                break;
        }

        return $"<color={color}>{valueStr}</color>";
    }
}