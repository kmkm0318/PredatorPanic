using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 희귀도에 따른 가중치 데이터
/// </summary>
[CreateAssetMenu(fileName = "RarityWeightData", menuName = "SO/Rarity/RarityWeightData", order = 0)]
public class RarityWeightData : ScriptableObject
{
    [Header("Rarity Weights")]
    [SerializeField] private WeightedList<Rarity> _baseWeights;
    [SerializeField] private WeightedList<Rarity> _luckWeights;

    #region 기본 가중치 딕셔너리
    private Dictionary<Rarity, float> _baseWeightDict;
    public Dictionary<Rarity, float> BaseWeightDict
    {
        get
        {
            if (_baseWeightDict == null)
            {
                _baseWeightDict = new Dictionary<Rarity, float>();
                foreach (var item in _baseWeights.Items)
                {
                    _baseWeightDict[item.Item] = item.Weight;
                }
            }
            return _baseWeightDict;
        }
    }
    #endregion

    #region 행운 가중치 딕셔너리
    private Dictionary<Rarity, float> _luckWeightDict;
    public Dictionary<Rarity, float> LuckWeightDict
    {
        get
        {
            if (_luckWeightDict == null)
            {
                _luckWeightDict = new Dictionary<Rarity, float>();
                foreach (var item in _luckWeights.Items)
                {
                    _luckWeightDict[item.Item] = item.Weight;
                }
            }
            return _luckWeightDict;
        }
    }
    #endregion

    public float GetTotalWeight(Rarity rarity, float luckStat)
    {
        float weight = 0f;

        if (BaseWeightDict.TryGetValue(rarity, out float baseWeight))
        {
            weight += baseWeight;
        }

        if (LuckWeightDict.TryGetValue(rarity, out float luckWeight))
        {
            weight += luckWeight * luckStat;
        }

        return weight;
    }
}