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

    #region 딕셔너리
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
    #endregion

    /// <summary>
    /// 희귀도에 해당하는 색상을 반환합니다
    /// 기본값은 흰색
    /// </summary>
    public Color GetRarityColor(Rarity rarity)
    {
        //딕셔너리에서 희귀도 데이터 검색
        if (RarityDataDict.TryGetValue(rarity, out var rarityData))
        {
            //딕셔너리에서 희귀도에 해당하는 색상 반환
            return rarityData.RarityColor;
        }
        else
        {
            //희귀도 데이터가 없을 경우 경고 로그 출력 후 기본 색상 반환
            Debug.LogWarning($"RarityDataList: Rarity {rarity} not found. Returning default color.");
            return Color.white;
        }
    }

    /// <summary>
    /// 희귀도 인덱스에 해당하는 색상을 반환합니다
    /// </summary>
    public Color GetRarityColor(int rarityIndex)
    {
        //인덱스가 유효한 범위 내에 있도록 클램프
        rarityIndex = Mathf.Clamp(rarityIndex, 0, System.Enum.GetValues(typeof(Rarity)).Length - 1);

        //인덱스를 희귀도로 변환
        Rarity rarity = (Rarity)rarityIndex;

        //희귀도에 해당하는 색상 반환
        return GetRarityColor(rarity);
    }
}