using UnityEngine;

public interface IBasicData
{
    string ID { get; }
    string Name { get; }
    string Description { get; }
    Sprite Icon { get; }
    Rarity Rarity { get; }
    int BasePrice { get; }
}