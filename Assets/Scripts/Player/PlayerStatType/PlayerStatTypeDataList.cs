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

    #region 딕셔너리
    private Dictionary<PlayerStatType, PlayerStatTypeData> _playerStatTypeDataDict;
    public Dictionary<PlayerStatType, PlayerStatTypeData> PlayerStatTypeDataDict
    {
        get
        {
            if (_playerStatTypeDataDict == null)
            {
                _playerStatTypeDataDict = new();
                foreach (var data in _playerStatTypeDatas)
                {
                    _playerStatTypeDataDict[data.PlayerStatType] = data;
                }
            }
            return _playerStatTypeDataDict;
        }
    }
    #endregion

    /// <summary>
    /// 플레이어 스탯 타입에 해당하는 데이터 반환
    /// </summary>
    public PlayerStatTypeData GetData(PlayerStatType type)
    {
        if (PlayerStatTypeDataDict.TryGetValue(type, out var statTypeData))
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