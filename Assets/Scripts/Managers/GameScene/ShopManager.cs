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
    [SerializeField] private int _refreshCostRaise = 1;
    [SerializeField] private AudioData _successAudioData;

    #region 레퍼런스
    private GameManager _gameManager;
    private Player _player;
    #endregion

    #region 상점 변수
    public int CurrentRefreshCost { get; private set; }
    #endregion

    #region 상점 상품 및 인벤토리 상품 리스트
    public List<IProduct> ShopProducts { get; private set; } = new();
    public List<WeaponInventoryProduct> WeaponInventoryProducts { get; private set; } = new();
    public List<ItemInventoryProduct> ItemInventoryProducts { get; private set; } = new();
    #endregion

    #region 이벤트
    public event Action<int> OnCurrentRefreshCostChanged;
    public event Action<List<IProduct>> OnShopProductsUpdated;
    public event Action<List<WeaponInventoryProduct>> OnWeaponInventoryProductsUpdated;
    public event Action<List<ItemInventoryProduct>> OnItemInventoryProductsUpdated;
    #endregion

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        _player = _gameManager.Player;

        RegisterEvents();

        InitShop();
    }

    //상점 초기화
    private void InitShop()
    {
        //초기 상점 상품 새로고침
        TryRefreshShopProducts(true);

        //플레이어의 무기 및 아이템 인벤토리 상품 초기화
        OnWeaponsChanged(_player.Weapons);
        OnItemsChanged(_player.Items);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        if (_player)
        {
            //플레이어 무기 및 아이템 변경 시 인벤토리 상품 업데이트
            _player.OnWeaponsChanged += OnWeaponsChanged;
            _player.OnItemsChanged += OnItemsChanged;
        }
    }

    private void UnregisterEvents()
    {
        if (_player)
        {
            _player.OnWeaponsChanged -= OnWeaponsChanged;
            _player.OnItemsChanged -= OnItemsChanged;
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void OnWeaponsChanged(List<Weapon> weapons)
    {
        //플레이어 무기 상품 목록 갱신
        WeaponInventoryProducts.Clear();

        foreach (var weapon in _player.Weapons)
        {
            //판매 가격은 비율에 따라 조정
            var weaponProduct = new WeaponInventoryProduct(weapon, _sellPriceRate);
            WeaponInventoryProducts.Add(weaponProduct);
        }

        //이벤트 호출
        OnWeaponInventoryProductsUpdated?.Invoke(WeaponInventoryProducts);
    }

    private void OnItemsChanged(List<Item> items)
    {
        //플레이어 아이템 상품 목록 갱신
        ItemInventoryProducts.Clear();

        foreach (var item in _player.Items)
        {
            //판매 가격은 비율에 따라 조정
            var itemProduct = new ItemInventoryProduct(item, _sellPriceRate);
            ItemInventoryProducts.Add(itemProduct);
        }

        //이벤트 호출
        OnItemInventoryProductsUpdated?.Invoke(ItemInventoryProducts);
    }
    #endregion

    #region 새로고침
    //판매 상품 새로고침
    public bool TryRefreshShopProducts(bool isInitial = false)
    {
        if (isInitial)
        {
            //처음에는 기본 코스트로 설정
            CurrentRefreshCost = _baseRefreshCost;

            //코스트 변경 이벤트 호출
            OnCurrentRefreshCostChanged?.Invoke(CurrentRefreshCost);
        }
        else
        {
            //처음이 아닐 시 플레이어의 이빨 소모 시도
            if (!_player.TrySpendTooth(CurrentRefreshCost))
            {
                //이빨이 부족하여 새로고침 실패
                return false;
            }

            //새로고침 성공 시 코스트 증가
            CurrentRefreshCost += _refreshCostRaise;

            //코스트 변경 이벤트 호출
            OnCurrentRefreshCostChanged?.Invoke(CurrentRefreshCost);

            //이펙트 사운드 재생
            AudioManager.Instance.PlaySfx(_successAudioData);
        }

        //기존 판매 상품 제거
        ShopProducts.Clear();

        //플레이어 행운 스탯 가져오기
        float luckStat = _player.PlayerStats.GetStat(PlayerStatType.Luck).FinalValue;

        //상품 개수 결정
        int weaponProductCount = UnityEngine.Random.Range(1, _shopSlotCount);
        int itemProductCount = _shopSlotCount - weaponProductCount;

        //무기 상품들 가져오기
        var randomWeaponDatas = GetRandomWeaponDatas(luckStat, weaponProductCount);

        //무기 상품 추가
        for (int i = 0; i < randomWeaponDatas.Count; i++)
        {
            var newWeaponProduct = new WeaponShopProduct(randomWeaponDatas[i]);
            ShopProducts.Add(newWeaponProduct);
        }

        //아이템 상품들 가져오기
        var randomItemDatas = GetRandomItemDatas(luckStat, itemProductCount);

        //아이템 상품 추가
        for (int i = 0; i < randomItemDatas.Count; i++)
        {
            var newItemProduct = new ItemShopProduct(randomItemDatas[i]);
            ShopProducts.Add(newItemProduct);
        }

        //판매 상품 업데이트 이벤트 호출
        OnShopProductsUpdated?.Invoke(ShopProducts);

        //새로고침 성공
        return true;
    }

    /// <summary>
    /// 행운 스탯과 개수에 따라 무기 데이터를 랜덤으로 가져오는 함수
    /// </summary>
    private List<WeaponData> GetRandomWeaponDatas(float luckStat, int count)
    {
        //무기 데이터 리스트 가져오기
        var weaponDatas = DataManager.Instance.WeaponDataList.WeaponDatas;

        //희귀도 가중치 데이터 가져오기
        var rarityWeightData = DataManager.Instance.RarityWeightData;

        //가중치 리스트 생성
        WeightedList<WeaponData> weightedWeaponDatas = new();

        //각 가중치에 따른 무기 데이터 추가
        foreach (var weaponData in weaponDatas)
        {
            float weight = rarityWeightData.GetTotalWeight(weaponData.Rarity, luckStat);

            weightedWeaponDatas.AddItem(new WeightedItem<WeaponData>(weaponData, weight));
        }

        //가중치에 따라 무기 데이터 선택
        var randomWeaponDatas = weightedWeaponDatas.GetRandomElements(count);

        return randomWeaponDatas;
    }

    /// <summary>
    /// 행운 스탯과 개수에 따라 아이템 데이터를 랜덤으로 가져오는 함수
    /// </summary>
    private List<ItemData> GetRandomItemDatas(float luckStat, int count)
    {
        //아이템 데이터 리스트 가져오기
        var itemDatas = DataManager.Instance.ItemDataList.ItemDatas;

        //희귀도 가중치 데이터 가져오기
        var rarityWeightData = DataManager.Instance.RarityWeightData;

        //가중치 리스트 생성
        WeightedList<ItemData> weightedItemDatas = new();

        //각 가중치에 따른 아이템 데이터 추가
        foreach (var itemData in itemDatas)
        {
            float weight = rarityWeightData.GetTotalWeight(itemData.Rarity, luckStat);

            weightedItemDatas.AddItem(new WeightedItem<ItemData>(itemData, weight));
        }

        //가중치에 따라 아이템 데이터 선택
        var randomItemDatas = weightedItemDatas.GetRandomElements(count);

        return randomItemDatas;
    }
    #endregion

    #region 구매 및 판매
    //상품 구매 시도
    public bool TryBuyProduct(IProduct product)
    {
        //이빨이 부족하면 구매 불가
        if (_player.Tooth < product.Price) return false;

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

        //구매 성공
        if (success)
        {
            //구매한 상품 제거
            ShopProducts.Remove(product);

            //판매 상품 업데이트 이벤트 호출
            OnShopProductsUpdated?.Invoke(ShopProducts);

            //이펙트 사운드 재생
            AudioManager.Instance.PlaySfx(_successAudioData);

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
        //이때 스탯은 적용하지 않음
        _player.AddTooth(sellPrice, false);

        //플레이어의 무기 목록에서 제거
        _player.RemoveWeapon(weapon);

        //이펙트 사운드 재생
        AudioManager.Instance.PlaySfx(_successAudioData);

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
        //이때 스탯은 적용하지 않음
        _player.AddTooth(sellPrice, false);

        //플레이어의 아이템 목록에서 제거
        _player.UnequipItem(item);

        //이펙트 사운드 재생
        AudioManager.Instance.PlaySfx(_successAudioData);

        //항상 성공. 추후 조건 추가 가능
        return true;
    }
    #endregion
}