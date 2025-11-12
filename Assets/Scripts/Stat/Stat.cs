using System.Collections.Generic;

/// <summary>
/// 스탯 클래스
/// 기본 값과 스탯 모디파이어 리스트를 보유
/// 최종 스탯 값 계산은 _isDirty 플래그를 통해 최적화
/// </summary>
public class Stat
{
    private float _baseValue;
    private readonly List<StatModifier> _statModifiers;
    private bool _isDirty;
    private float _finalValue;
    public float FinalValue
    {
        get
        {
            if (_isDirty)
            {
                CalculateFinalValue();
                _isDirty = false;
            }
            return _finalValue;
        }
    }

    public Stat(float baseValue)
    {
        _baseValue = baseValue;
        _statModifiers = new();
        _isDirty = true;
    }

    /// <summary>
    /// 스탯 모디파이어 추가
    /// </summary>
    public void AddModifier(StatModifier mod)
    {
        _statModifiers.Add(mod);
        _isDirty = true;
    }

    /// <summary>
    /// 스탯 모디파이어 제거
    /// </summary>
    public bool RemoveModifier(StatModifier mod)
    {
        if (_statModifiers.Remove(mod))
        {
            _isDirty = true;
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
            _isDirty = true;
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

        _finalValue = _baseValue + finalFlat;
        _finalValue *= 1 + finalPercentAdd;
        _finalValue *= finalPercentMult;
    }
}