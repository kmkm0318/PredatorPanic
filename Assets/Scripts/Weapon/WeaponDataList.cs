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

    #region ID를 통한 딕셔너리
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

    #region 희귀도 별 무기 데이터 리스트
    private Dictionary<Rarity, List<WeaponData>> _rarityWeaponDataDict;
    public Dictionary<Rarity, List<WeaponData>> RarityWeaponDataDict
    {
        get
        {
            if (_rarityWeaponDataDict == null)
            {
                _rarityWeaponDataDict = new();
                foreach (var data in _weaponDatas)
                {
                    if (!_rarityWeaponDataDict.ContainsKey(data.Rarity))
                    {
                        _rarityWeaponDataDict[data.Rarity] = new List<WeaponData>();
                    }
                    _rarityWeaponDataDict[data.Rarity].Add(data);
                }
            }
            return _rarityWeaponDataDict;
        }
    }
    #endregion

    /// <summary>
    /// ID로 무기 데이터 가져오기
    /// </summary>
    public WeaponData GetData(string id)
    {
        if (WeaponDataDict.TryGetValue(id, out var weaponData))
        {
            return weaponData;
        }
        Debug.LogError($"WeaponData with ID {id} not found.");
        return null;
    }

    /// <summary>
    /// 희귀도로 무기 데이터 리스트 가져오기
    /// </summary>
    public List<WeaponData> GetRarityDatas(Rarity rarity)
    {
        if (RarityWeaponDataDict.TryGetValue(rarity, out var weaponDataList))
        {
            return weaponDataList;
        }
        Debug.LogWarning($"No WeaponData found for Rarity {rarity}.");
        return new List<WeaponData>();
    }
}