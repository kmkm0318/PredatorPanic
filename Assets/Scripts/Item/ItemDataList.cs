using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터 리스트 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "ItemDataList", menuName = "SO/Item/ItemDataList", order = 0)]
public class ItemDataList : ScriptableObject
{
    [SerializeField] private List<ItemData> _itemDatas;
    public List<ItemData> ItemDatas => _itemDatas;

    #region 희귀도에 따른 아이템 데이터 리스트
    private Dictionary<Rarity, List<ItemData>> _rarityItemDataDict;
    public Dictionary<Rarity, List<ItemData>> RarityItemDataDict
    {
        get
        {
            if (_rarityItemDataDict == null)
            {
                _rarityItemDataDict = new();
                foreach (var data in _itemDatas)
                {
                    if (!_rarityItemDataDict.ContainsKey(data.Rarity))
                    {
                        _rarityItemDataDict[data.Rarity] = new List<ItemData>();
                    }
                    _rarityItemDataDict[data.Rarity].Add(data);
                }
            }
            return _rarityItemDataDict;
        }
    }
    #endregion

    /// <summary>
    /// 특정 희귀도의 아이템 데이터 리스트 반환
    /// </summary>
    public List<ItemData> GetRarityDatas(Rarity rarity)
    {
        if (RarityItemDataDict.TryGetValue(rarity, out var itemDatas))
        {
            return itemDatas;
        }
        Debug.LogWarning($"ItemDataList: No item datas found for rarity {rarity}");
        return null;
    }
}