using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진화 데이터 리스트 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "EvolutionDataList", menuName = "SO/Evolution/EvolutionDataList", order = 0)]
public class EvolutionDataList : ScriptableObject
{
    [Header("Evolution Data List")]
    [SerializeField] private List<EvolutionData> _evolutionDatas = new();
    public List<EvolutionData> EvolutionDatas => _evolutionDatas;

    #region ID를 통한 접근
    private Dictionary<string, EvolutionData> _evolutionDataDict;
    public Dictionary<string, EvolutionData> EvolutionDataDict
    {
        get
        {
            if (_evolutionDataDict == null)
            {
                _evolutionDataDict = new();
                foreach (var data in _evolutionDatas)
                {
                    _evolutionDataDict[data.ID] = data;
                }
            }
            return _evolutionDataDict;
        }
    }
    #endregion
}