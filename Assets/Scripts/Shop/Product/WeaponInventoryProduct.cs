using UnityEngine;

/// <summary>
/// 무기 판매 상품 클래스
/// 플레이어가 소유한 무기를 상점에서 판매할 수 있도록 함
/// </summary>
public class WeaponInventoryProduct : IProduct
{
    public Weapon Weapon { get; private set; }
    public Sprite Icon { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Rarity Rarity { get; private set; }
    public int Price { get; private set; }

    public WeaponInventoryProduct(Weapon weapon, float priceRate = 1f)
    {
        var weaponData = weapon.WeaponData;

        Weapon = weapon;
        Icon = weaponData.Icon;
        Name = weaponData.WeaponName;
        Description = Weapon.GetDescription();
        Rarity = weaponData.Rarity;
        Price = Mathf.CeilToInt(weaponData.BasePrice * priceRate);
    }
}