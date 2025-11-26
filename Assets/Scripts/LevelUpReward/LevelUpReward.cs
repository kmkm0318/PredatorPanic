using System.Collections.Generic;

/// <summary>
/// 레벨 업 보상 클래스
/// 런타임에 플레이어에게 적용되는 보상 효과들을 담고 있습니다.
/// </summary>
public class LevelUpReward
{
    //데이터
    public LevelUpRewardData RewardData { get; private set; }

    //효과 리스트
    private List<Effect> _effects = new();

    //생성자.
    //효과들을 리스트로 생성
    public LevelUpReward(LevelUpRewardData rewardData)
    {
        RewardData = rewardData;
        foreach (var effectData in RewardData.EffectDatas)
        {
            var effect = effectData.GetEffect();
            _effects.Add(effect);
        }
    }

    //효과 적용
    public void Apply(Player player)
    {
        foreach (var effect in _effects)
        {
            effect.Apply(player);
        }
    }

    ///효과 제거
    public void Remove(Player player)
    {
        foreach (var effect in _effects)
        {
            effect.Remove(player);
        }
    }
}