/// <summary>
/// 스탯 버프 효과 클래스
/// </summary>
public class StatBuffEffect : Effect
{
    //데이터 저장
    private StatBuffEffectData _data;

    public StatBuffEffect(EffectData effectData) : base(effectData)
    {
        _data = effectData as StatBuffEffectData;
    }

    public override void Apply(Player player)
    {
        //목표 스탯
        var stat = player.PlayerStats.GetStat(_data.StatType);

        //스탯 모디파이어 추가
        stat.AddModifier(new StatModifier(_data.BuffAmount, _data.ModifierType, this));
    }

    public override void Remove(Player player)
    {
        //목표 스탯
        var stat = player.PlayerStats.GetStat(_data.StatType);

        //스탯 모디파이어 제거
        stat.RemoveAllModifiersFromSource(this);
    }
}