using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨 업 보상 데이터 스크립터블 오브젝트
/// 레벨 업 시 플레이어가 받을 수 있는 보상 정보를 담고 있습니다.
/// </summary>
[CreateAssetMenu(fileName = "LevelUpRewardData", menuName = "SO/LevelUpReward/LevelUpRewardData", order = 0)]
public class LevelUpRewardData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string _rewardName;
    [SerializeField] private Sprite _rewardIcon;
    public string RewardName => _rewardName;
    public Sprite RewardIcon => _rewardIcon;

    [Header("Effect Data")]
    [SerializeField] private List<LevelUpRewardEffect> _rewardEffects;
    public List<LevelUpRewardEffect> RewardEffects => _rewardEffects;
}
