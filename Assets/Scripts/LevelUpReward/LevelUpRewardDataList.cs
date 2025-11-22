using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpRewardDataList", menuName = "SO/LevelUpReward/LevelUpRewardDataList", order = 0)]
public class LevelUpRewardDataList : ScriptableObject
{
    [SerializeField] private List<LevelUpRewardData> _levelUpRewardDatas;
    public List<LevelUpRewardData> LevelUpRewardDatas => _levelUpRewardDatas;
}