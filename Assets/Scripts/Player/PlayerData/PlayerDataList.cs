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

    #region 딕셔너리
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

    /// <summary>
    /// ID로 플레이어 데이터 가져오기
    /// </summary>
    public PlayerData GetData(string id)
    {
        if (PlayerDataDict.TryGetValue(id, out var playerData))
        {
            return playerData;
        }
        Debug.LogError($"PlayerData with ID {id} not found.");
        return null;
    }
}