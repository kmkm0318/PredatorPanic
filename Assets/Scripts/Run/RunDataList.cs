using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 런 데이터 리스트 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "RunDataList", menuName = "SO/Run/RunDataList", order = 0)]
public class RunDataList : ScriptableObject
{
    [SerializeField] private List<RunData> _runDatas;
    public List<RunData> RunDatas => _runDatas;

    #region 딕셔너리
    private Dictionary<string, RunData> _runDataDict;
    public Dictionary<string, RunData> RunDataDict
    {
        get
        {
            if (_runDataDict == null)
            {
                _runDataDict = new Dictionary<string, RunData>();
                foreach (var runData in _runDatas)
                {
                    _runDataDict[runData.ID] = runData;
                }
            }
            return _runDataDict;
        }
    }
    #endregion

    public RunData GetData(string id)
    {
        if (RunDataDict.TryGetValue(id, out var runData))
        {
            return runData;
        }

        Debug.LogError($"RunData with ID {id} not found.");
        return null;
    }
}