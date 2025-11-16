using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스탯 클래스
/// 기본 값과 스탯 모디파이어 리스트를 보유
/// 값 변경 시 이벤트 발생
/// </summary>
public class Stat
{
    //초기값
    public float BaseValue { get; private set; }
    //최종값
    public float FinalValue { get; private set; }
    //스탯 모디파이어 리스트
    private readonly List<StatModifier> _statModifiers;
    //값 변경 이벤트
    public event Action<float> OnValueChanged;

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        _statModifiers = new();
        CalculateFinalValue();
    }

    /// <summary>
    /// 스탯 모디파이어 추가
    /// </summary>
    public void AddModifier(StatModifier mod)
    {
        _statModifiers.Add(mod);
        CalculateFinalValue();
    }

    /// <summary>
    /// 스탯 모디파이어 제거
    /// </summary>
    public bool RemoveModifier(StatModifier mod)
    {
        if (_statModifiers.Remove(mod))
        {
            CalculateFinalValue();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 소스에 해당하는 모든 스탯 모디파이어 제거
    /// </summary>
    public bool RemoveAllModifiersFromSource(object source)
    {
        int numRemoved = _statModifiers.RemoveAll(mod => mod.Source == source);
        if (numRemoved > 0)
        {
            CalculateFinalValue();
            return true;
        }
        return false;
    }

    private void CalculateFinalValue()
    {
        float finalFlat = 0f;
        float finalPercentAdd = 0f;
        float finalPercentMult = 1f;

        // 적용 순서: Flat -> PercentAdd -> PercentMult
        foreach (var mod in _statModifiers)
        {
            switch (mod.Type)
            {
                case StatModifierType.Flat:
                    finalFlat += mod.Value;
                    break;
                case StatModifierType.PercentAdd:
                    finalPercentAdd += mod.Value;
                    break;
                case StatModifierType.PercentMult:
                    finalPercentMult *= 1 + mod.Value;
                    break;
            }
        }

        FinalValue = BaseValue + finalFlat;
        FinalValue *= 1 + finalPercentAdd;
        FinalValue *= finalPercentMult;

        FinalValue = Mathf.Max(0, FinalValue); // 음수 방지

        OnValueChanged?.Invoke(FinalValue);
    }
}