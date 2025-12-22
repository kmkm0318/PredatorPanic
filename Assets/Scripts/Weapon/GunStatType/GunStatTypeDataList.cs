using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총기 스탯 타입 데이터 리스트
/// </summary>
[CreateAssetMenu(fileName = "GunStatTypeDataList", menuName = "SO/Weapon/Gun/GunStatTypeDataList", order = 0)]
public class GunStatTypeDataList : ScriptableObject
{
    [Header("Gun Stat Type Data List")]
    [SerializeField] private List<GunStatTypeData> _gunStatTypeDatas;
    public List<GunStatTypeData> GunStatTypeDatas => _gunStatTypeDatas;

    #region 딕셔너리
    private Dictionary<GunStatType, GunStatTypeData> _gunStatTypeDataDict;
    public Dictionary<GunStatType, GunStatTypeData> GunStatTypeDataDict
    {
        get
        {
            if (_gunStatTypeDataDict == null)
            {
                _gunStatTypeDataDict = new();
                foreach (var data in _gunStatTypeDatas)
                {
                    _gunStatTypeDataDict[data.GunStatType] = data;
                }
            }
            return _gunStatTypeDataDict;
        }
    }
    #endregion
}