using UnityEngine;

/// <summary>
/// 스탯 버프 효과 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "StatBuffEffectData", menuName = "SO/Effect/StatBuffEffectData", order = 0)]
public class StatBuffEffectData : EffectData
{
    [Header("Stat Buff Info")]
    [field: SerializeField] public PlayerStatType StatType { get; private set; }
    [field: SerializeField] public StatModifierType ModifierType { get; private set; }
    [field: SerializeField] public float BuffAmount { get; private set; }

    public override Effect GetEffect()
    {
        return new StatBuffEffect(this);
    }

    override public string GetDescription()
    {
        //목표 스탯 이름 가져오기
        var statData = DataManager.Instance.PlayerStatTypeDataList.GetData(StatType);

        //설명 반환
        return $"{statData.StatName} {StringUtility.GetModifierDescription(ModifierType, BuffAmount)}";
    }
}