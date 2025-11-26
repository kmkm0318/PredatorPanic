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
}