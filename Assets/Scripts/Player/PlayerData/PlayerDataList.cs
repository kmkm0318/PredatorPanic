using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터 리스트 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "PlayerDataList", menuName = "SO/Player/PlayerDataList", order = 0)]
public class PlayerDataList : ScriptableObject
{
    [SerializeField] private List<PlayerData> _playerDatas = new();
    public List<PlayerData> PlayerDatas => _playerDatas;

    #region ID를 통한 접근
    private Dictionary<string, PlayerData> _playerDataDict;
    public Dictionary<string, PlayerData> PlayerDataDict
    {
        get
        {
            if (_playerDataDict == null)
            {
                _playerDataDict = new();
                foreach (var data in _playerDatas)
                {
                    _playerDataDict[data.ID] = data;
                }
            }
            return _playerDataDict;
        }
    }
    #endregion
}