using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레벨 업 보상 데이터 리스트 스크립터블 오브젝트
/// 레벨 업 시 플레이어가 받을 수 있는 보상 데이터들의 목록을 담고 있습니다.
/// </summary>
[CreateAssetMenu(fileName = "LevelUpRewardDataList", menuName = "SO/LevelUpReward/LevelUpRewardDataList", order = 0)]
public class LevelUpRewardDataList : ScriptableObject
{
    [SerializeField] private List<LevelUpRewardData> _levelUpRewardDatas;
    public List<LevelUpRewardData> LevelUpRewardDatas => _levelUpRewardDatas;

    #region 희귀도에 따른 보상 데이터 리스트
    private Dictionary<Rarity, List<LevelUpRewardData>> _rarityRewardDataDict;
    public Dictionary<Rarity, List<LevelUpRewardData>> RarityRewardDataDict
    {
        get
        {
            if (_rarityRewardDataDict == null)
            {
                _rarityRewardDataDict = new();
                foreach (var data in _levelUpRewardDatas)
                {
                    if (!_rarityRewardDataDict.ContainsKey(data.Rarity))
                    {
                        _rarityRewardDataDict[data.Rarity] = new List<LevelUpRewardData>();
                    }
                    _rarityRewardDataDict[data.Rarity].Add(data);
                }
            }
            return _rarityRewardDataDict;
        }
    }
    #endregion

    /// <summary>
    /// 특정 희귀도의 보상 데이터 리스트 반환
    /// </summary>
    public List<LevelUpRewardData> GetRarityDatas(Rarity rarity)
    {
        if (RarityRewardDataDict.TryGetValue(rarity, out var rewardDatas))
        {
            return rewardDatas;
        }
        Debug.LogWarning($"LevelUpRewardDataList: No reward datas found for rarity {rarity}");
        return null;
    }
}