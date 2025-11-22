using System;
using UnityEngine;

/// <summary>
/// 레벨 업 보상 효과 클래스
/// 레벨 업 보상으로 플레이어에게 적용되는 다양한 효과들을 정의합니다.
/// </summary>
[Serializable]
public class LevelUpRewardEffect
{
    [Header("Enhance Player Stat Effect Info")]
    [SerializeField] private PlayerStatType _statType;
    [SerializeField] private StatModifierType _modifierType;
    [SerializeField] private float _modifierValue;

    public void ApplyEffect(Player player)
    {
        player.PlayerStats.GetStat(_statType).AddModifier(new StatModifier(_modifierValue, _modifierType, this));
    }

    public string GetDescription()
    {
        var statData = DataManager.Instance.PlayerStatTypeDataList.GetData(_statType);

        return statData.StatName + StringUtility.GetModifierDescription(_modifierType, _modifierValue);
    }
}