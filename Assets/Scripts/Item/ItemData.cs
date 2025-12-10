using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _basePrice;
    public string ItemName => _itemName;
    public Sprite Icon => _icon;
    public int BasePrice => _basePrice;

    [Header("Item Effect Data")]
    [SerializeField] private List<EffectData> _effectDatas;
    public List<EffectData> EffectDatas => _effectDatas;

    //설명 반환
    public string GetDescription()
    {
        List<string> descriptions = new();
        foreach (var effectData in _effectDatas)
        {
            descriptions.Add(effectData.GetDescription());
        }
        return string.Join("\n", descriptions);
    }
}