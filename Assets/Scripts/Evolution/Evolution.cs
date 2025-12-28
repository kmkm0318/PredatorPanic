using System.Collections.Generic;

/// <summary>
/// 진화 클래스
/// 플레이어가 영구적으로 획득한 효과들을 관리
/// </summary>
public class Evolution
{
    #region 데이터
    private EvolutionData _evolutionData;
    #endregion

    #region 변수
    private readonly List<Effect> _effects = new();
    #endregion

    #region 초기화
    public Evolution(EvolutionData evolutionData)
    {
        _evolutionData = evolutionData;

        InitEffects();
    }

    private void InitEffects()
    {
        foreach (var data in _evolutionData.Effects)
        {
            var effect = data.GetEffect();
            _effects.Add(effect);
        }
    }
    #endregion

    #region 효과 적용 및 해제
    public void ApplyEffect(Player player)
    {
        //모든 효과 적용
        foreach (var effect in _effects)
        {
            effect.Apply(player);
        }
    }

    public void RemoveEffect(Player player)
    {
        //모든 효과 해제
        foreach (var effect in _effects)
        {
            effect.Remove(player);
        }
    }
    #endregion
}