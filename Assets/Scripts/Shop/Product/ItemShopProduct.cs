using UnityEngine;

/// <summary>
/// 아이템 구매 상품 클래스
/// 아이템 데이터를 기반으로 상점에서 구매 가능한 상품을 나타냄
/// </summary>
public class ItemShopProduct : IProduct
{
    public ItemData ItemData { get; private set; }
    public Sprite Icon { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Price { get; private set; }

    public ItemShopProduct(ItemData itemData, float priceRate = 1f)
    {
        ItemData = itemData;
        Icon = itemData.Icon;
        Name = itemData.ItemName;
        Description = itemData.GetDescription();
        Price = Mathf.CeilToInt(itemData.BasePrice * priceRate);
    }
}
