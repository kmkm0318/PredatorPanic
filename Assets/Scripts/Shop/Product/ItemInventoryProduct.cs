using UnityEngine;

/// <summary>
/// 아이템 판매 상품 클래스
/// 플레이어가 소유한 아이템을 상점에서 판매할 수 있도록 함
/// </summary>
public class ItemInventoryProduct : IProduct
{
    public Item Item { get; private set; }
    public Sprite Icon { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Rarity Rarity { get; private set; }
    public int Price { get; private set; }

    public ItemInventoryProduct(Item item, float priceRate = 1f)
    {
        var itemData = item.ItemData;

        Item = item;
        Icon = itemData.Icon;
        Name = itemData.ItemName;
        Description = Item.GetDescription();
        Rarity = itemData.Rarity;
        Price = Mathf.CeilToInt(itemData.BasePrice * priceRate);
    }
}