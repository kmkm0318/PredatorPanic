/// <summary>
/// 문자열 유틸리티 클래스
/// </summary>
public static class StringUtility
{
    /// <summary>
    /// 스탯 모디파이어 타입에 따른 value의 설명을 반환합니다.
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
                valueStr = $"{(value >= 0 ? "+" : "")}{value.ToPercent()}";
                break;
            case StatModifierType.PercentMult:
                color = value >= 1 ? "green" : "red";
                valueStr = $"x{value}";
                break;
        }

        return $"<color={color}>{valueStr}</color>";
    }

    /// <summary>
    /// 숫자를 K, M, B 단위로 포맷팅하여 반환합니다.
    /// </summary>
    public static string GetFormatedNumber(this float number)
    {
        if (number >= 1e9f)
            return (number / 1e9f).ToString("0.##") + "B";
        else if (number >= 1e6f)
            return (number / 1e6f).ToString("0.##") + "M";
        else if (number >= 1e3f)
            return (number / 1e3f).ToString("0.##") + "K";
        else
            return number.ToString("0");
    }

    /// <summary>
    /// 숫자를 백분율 문자열로 변환하여 반환합니다.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string ToPercent(this float number)
    {
        return (number * 100).ToString("0.##") + "%";
    }
}