
/// <summary>
/// 효과 클래스
/// 런타임에 플레이어에게 적용되는 실제 효과들을 정의
/// </summary>
public abstract class Effect
{
    public EffectData EffectData { get; private set; }

    public Effect(EffectData effectData)
    {
        EffectData = effectData;
    }

    /// <summary>
    /// 효과 적용 메서드
    /// </summary>
    public abstract void Apply(Player player);

    /// <summary>
    /// 효과 제거 메서드
    /// </summary>
    public abstract void Remove(Player player);

    /// <summary>
    /// 효과 설명 반환 메서드
    /// 기본적으로 데이터의 설명을 그대로 반환합니다.
    /// 런타임에 동적으로 변경된 설명이 필요한 경우 오버라이드합니다.
    /// </summary>
    public virtual string GetDescription()
    {
        return EffectData.GetDescription();
    }
}