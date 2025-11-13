using System;
using System.Collections.Generic;

/// <summary>
/// 스탯을 모아두는 제너럴 클래스
/// T는 스탯 타입 Enum
/// </summary>
public class Stats<T> where T : Enum
{
    private Dictionary<T, Stat> _stats = new();

    public Stats(List<StatEntity<T>> initialStats)
    {
        foreach (var statEntity in initialStats)
        {
            _stats[statEntity.StatType] = new Stat(statEntity.Value);
        }
    }

    public Stat GetStat(T statType)
    {
        if (!_stats.TryGetValue(statType, out var stat))
        {
            _stats[statType] = new Stat(0);
            stat = _stats[statType];
        }
        return stat;
    }
}