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
    private int _level = 0;
    #endregion

    #region 초기화
    public Evolution(EvolutionData evolutionData, int level)
    {
        _evolutionData = evolutionData;

        _level = level;

        InitEffects();
    }

    private void InitEffects()
    {
        //데이터 리스트 가져오기
        var datas = _evolutionData.GetEffectsByLevel(_level);

        //이펙트 생성 및 추가
        foreach (var data in datas)
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