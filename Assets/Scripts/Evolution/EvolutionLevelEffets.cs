using System;
using System.Collections.Generic;

/// <summary>
/// 진화 레벨별 효과 클래스
/// </summary>
[Serializable]
public class EvolutionLevelEffets
{
    public List<EffectData> Effects;

    public EvolutionLevelEffets(List<EffectData> effects)
    {
        Effects = effects;
    }
}