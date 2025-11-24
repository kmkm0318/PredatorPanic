using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList", menuName = "SO/Item/ItemDataList", order = 0)]
public class ItemDataList : ScriptableObject
{
    [SerializeField] private List<ItemData> _itemDatas;
    public List<ItemData> ItemDatas => _itemDatas;
}