using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 스탯 타입 데이터 리스트
/// </summary>
[CreateAssetMenu(fileName = "PlayerStatTypeDataList", menuName = "SO/Player/PlayerStatTypeDataList", order = 0)]
public class PlayerStatTypeDataList : ScriptableObject
{
    [Header("Player Stat Type Data List")]
    [SerializeField] private List<PlayerStatTypeData> _playerStatTypeDatas;
    public List<PlayerStatTypeData> PlayerStatTypeDatas => _playerStatTypeDatas;

    // 빠르게 찾기 위한 딕셔너리
    private Dictionary<PlayerStatType, PlayerStatTypeData> _playerStatTypeDataDict;

    // 최초 접근 시 딕셔너리 생성
    // PlayerStatType으로 PlayerStatTypeData 조회
    public PlayerStatTypeData GetData(PlayerStatType type)
    {
        if (_playerStatTypeDataDict == null)
        {
            _playerStatTypeDataDict = new();
            foreach (var data in _playerStatTypeDatas)
            {
                _playerStatTypeDataDict[data.PlayerStatType] = data;
            }
        }

        if (_playerStatTypeDataDict.TryGetValue(type, out var statTypeData))
        {
            return statTypeData;
        }
        else
        {
            $"PlayerStatTypeData not found. stat type: {type}".LogError();
            return null;
        }
    }
}