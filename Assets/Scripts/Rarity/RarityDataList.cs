using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 희귀도 데이터 리스트 클래스
/// 여러 희귀도 데이터를 하나의 리스트로 관리합니다
/// 딕셔너리를 통해 데이터에 쉽게 접근할 수 있습니다
/// </summary>
[CreateAssetMenu(fileName = "RarityDataList", menuName = "SO/Rarity/RarityDataList", order = 0)]
public class RarityDataList : ScriptableObject
{
    [Header("Rarity Data List")]
    [SerializeField] private List<RarityData> _rarityDatas;

    // 희귀도 데이터 딕셔너리
    private Dictionary<Rarity, RarityData> _rarityDataDict;
    public Dictionary<Rarity, RarityData> RarityDataDict
    {
        get
        {
            if (_rarityDataDict == null)
            {
                _rarityDataDict = new();
                foreach (var rarityData in _rarityDatas)
                {
                    _rarityDataDict[rarityData.Rarity] = rarityData;
                }
            }
            return _rarityDataDict;
        }
    }
}