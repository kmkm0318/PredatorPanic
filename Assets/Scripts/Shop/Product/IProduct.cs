using UnityEngine;

/// <summary>
/// 상품 인터페이스
/// 상점에서 구매하는 상품이나 플레이어가 판매할 무기, 아이템 상품
/// </summary>
public interface IProduct
{
    Sprite Icon { get; }
    string Name { get; }
    string Description { get; }
    int Price { get; }
}
