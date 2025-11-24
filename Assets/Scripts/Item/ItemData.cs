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
}