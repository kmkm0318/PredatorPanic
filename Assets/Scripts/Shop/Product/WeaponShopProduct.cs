using UnityEngine;

/// <summary>
/// 무기 구매 상품 클래스
/// 무기 데이터를 기반으로 상점에서 구매 가능한 상품을 나타냄
/// </summary>
public class WeaponShopProduct : IProduct
{
    public WeaponData WeaponData { get; private set; }
    public Sprite Icon { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Rarity Rarity { get; private set; }
    public int Price { get; private set; }

    public WeaponShopProduct(WeaponData weaponData, float priceRate = 1f)
    {
        WeaponData = weaponData;
        Icon = weaponData.Icon;
        Name = weaponData.WeaponName;
        Description = weaponData.Description;
        Rarity = weaponData.Rarity;
        Price = Mathf.CeilToInt(weaponData.BasePrice * priceRate);
    }
}
