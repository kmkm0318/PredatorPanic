/// <summary>
/// 스탯 모디파이어 클래스
/// 스탯에 추가되어 최종값에 영향을 미침
/// 값, 타입, 출처 포함
/// </summary>
public class StatModifier
{
    public float Value { get; private set; }
    public StatModifierType Type { get; private set; }
    public object Source { get; private set; }

    public StatModifier(float value, StatModifierType type, object source)
    {
        Value = value;
        Type = type;
        Source = source;
    }
}