using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상점 관리자 클래스
/// </summary>
public class ShopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    [SerializeField] private int _shopSlotCount = 4;
    [SerializeField] private float _sellPriceRate = 0.5f;
    [SerializeField] private int _baseRefreshCost = 5;

    #region 레퍼런스
    private Player _player;
    #endregion

    #region 상점 상품 및 인벤토리 상품 리스트
    public List<IProduct> BuyProducts { get; private set; } = new();
    public List<WeaponInventoryProduct> WeaponSellProducts { get; private set; } = new();
    public List<ItemInventoryProduct> ItemSellProducts { get; private set; } = new();
    #endregion

    #region 새로고침 코스트
    public int CurrentRefreshCost { get; private set; }
    #endregion

    //상호작용하기 위한 플레이어 전달받기
    public void Init(Player player)
    {
        _player = player;
    }

    #region 새로고침
    //상점 새로고침
    public void RefreshAllProduct()
    {
        TryRefreshShopProducts(true);
        RefreshWeaponInventoryProducts();
        RefreshItemInventoryProducts();
    }

    //판매 상품 새로고침
    public bool TryRefreshShopProducts(bool isInitial = false)
    {
        if (isInitial)
        {
            //처음에는 기본 코스트로 설정
            CurrentRefreshCost = _baseRefreshCost;
        }
        else
        {
            if (!_player.TrySpendTooth(CurrentRefreshCost))
            {
                //이빨이 부족하여 새로고침 실패
                return false;
            }
            //새로고침 코스트 증가
            CurrentRefreshCost++;
        }

        //기존 판매 상품 제거
        BuyProducts.Clear();

        int weaponProductCount = UnityEngine.Random.Range(1, _shopSlotCount); //무기 상품 개수 랜덤 결정
        int itemProductCount = _shopSlotCount - weaponProductCount; //나머지는 아이템 상품 개수

        //무기 상품들 가져오기
        var weaponDatas = DataManager.Instance.WeaponDataList.WeaponDatas;
        var randomWeaponDatas = weaponDatas.GetRandomElements(weaponProductCount);

        //무기 상품 추가
        for (int i = 0; i < randomWeaponDatas.Count; i++)
        {
            var newWeaponProduct = new WeaponShopProduct(randomWeaponDatas[i]);
            BuyProducts.Add(newWeaponProduct);
        }

        //아이템 상품들 가져오기
        var itemDatas = DataManager.Instance.ItemDataList.ItemDatas;
        var randomItemDatas = itemDatas.GetRandomElements(itemProductCount);

        //아이템 상품 추가
        for (int i = 0; i < randomItemDatas.Count; i++)
        {
            var newItemProduct = new ItemShopProduct(randomItemDatas[i]);
            BuyProducts.Add(newItemProduct);
        }

        //새로고침 성공
        return true;
    }

    //플레이어 무기 새로고침
    public void RefreshWeaponInventoryProducts()
    {
        //플레이어 무기 상품 목록 갱신
        WeaponSellProducts.Clear();
        foreach (var weapon in _player.Weapons)
        {
            //판매 가격은 비율에 따라 조정
            var weaponProduct = new WeaponInventoryProduct(weapon, _sellPriceRate);
            WeaponSellProducts.Add(weaponProduct);
        }
    }

    //플레이어 아이템 새로고침
    public void RefreshItemInventoryProducts()
    {
        //플레이어 아이템 상품 목록 갱신
        ItemSellProducts.Clear();

        foreach (var item in _player.Items)
        {
            //판매 가격은 비율에 따라 조정
            var itemProduct = new ItemInventoryProduct(item, _sellPriceRate);
            ItemSellProducts.Add(itemProduct);
        }
    }
    #endregion

    #region 구매 및 판매
    //상품 구매 시도
    public bool TryBuyProduct(IProduct product)
    {
        //플레이어의 이빨을 통해서 구매 시도
        if (!_player.TrySpendTooth(product.Price))
        {
            //구매 실패
            return false;
        }

        bool success = false;

        //무기 추가 시도
        if (product is WeaponShopProduct weaponProduct)
        {
            success = _player.TryAddWeapon(weaponProduct.WeaponData);
        }
        //아이템 추가 시도
        else if (product is ItemShopProduct itemProduct)
        {
            success = _player.TryEquipItem(itemProduct.ItemData);
        }

        if (success)
        {
            //구매 성공 시 상품 제거
            BuyProducts.Remove(product);
            return true;
        }

        //구매 실패
        return false;
    }

    //무기 판매 시도
    public bool TrySellWeaponProduct(WeaponInventoryProduct weaponProduct)
    {
        //무기 가져오기
        var weapon = weaponProduct.Weapon;

        //판매 가격은 비율에 따라 조정되어 있음
        int sellPrice = weaponProduct.Price;

        //무기 판매는 이빨 재화로 제공
        _player.AddTooth(sellPrice);

        //플레이어의 무기 목록에서 제거
        _player.RemoveWeapon(weapon);

        //항상 성공. 추후 조건 추가 가능
        return true;
    }

    //아이템 판매 시도
    public bool TrySellItemProduct(ItemInventoryProduct itemProduct)
    {
        //아이템 가져오기
        var item = itemProduct.Item;

        //판매 가격은 비율에 따라 조정되어 있음
        int sellPrice = itemProduct.Price;

        //아이템 판매는 이빨 재화로 제공
        _player.AddTooth(sellPrice);

        //플레이어의 아이템 목록에서 제거
        _player.UnequipItem(item);

        //항상 성공. 추후 조건 추가 가능
        return true;
    }
    #endregion
}