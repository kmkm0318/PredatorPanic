using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 데이터 리스트 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "WeaponDataList", menuName = "SO/Weapon/WeaponDataList", order = 0)]
public class WeaponDataList : ScriptableObject
{
    [SerializeField] private List<WeaponData> _weaponDatas;
    public List<WeaponData> WeaponDatas => _weaponDatas;

    #region ID를 통한 접근
    private Dictionary<string, WeaponData> _weaponDataDict;
    public Dictionary<string, WeaponData> WeaponDataDict
    {
        get
        {
            if (_weaponDataDict == null)
            {
                _weaponDataDict = new();
                foreach (var data in _weaponDatas)
                {
                    _weaponDataDict[data.ID] = data;
                }
            }
            return _weaponDataDict;
        }
    }
    #endregion
}