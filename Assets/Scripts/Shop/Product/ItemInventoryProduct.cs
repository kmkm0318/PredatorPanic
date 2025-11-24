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
    public int Price { get; private set; }

    public ItemInventoryProduct(Item item, float priceRate = 1f)
    {
        Item = item;
        var itemData = item.ItemData;
        Icon = itemData.Icon;
        Name = itemData.ItemName;
        Price = Mathf.CeilToInt(itemData.BasePrice * priceRate);
    }
}