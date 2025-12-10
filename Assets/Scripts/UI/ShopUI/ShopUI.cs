using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// 상점 UI 클래스
/// 상점에서 상품을 구매하거나 플레이어의 무기, 아이템을 판매할 수 있습니다
/// 판매 아이템을 새로고침하거나 다음 라운드로 넘어갈 수 있습니다
/// </summary>
public class ShopUI : ShowHideUI
{
    [Header("Product Prefab and Parents")]
    [SerializeField] private ProductSlotUI _productSlotUIPrefab;
    [SerializeField] private Transform _shopProductParent;
    [SerializeField] private Transform _weaponInventoryProductParent;
    [SerializeField] private Transform _itemInventoryProductParent;

    [Header("Refresh")]
    [SerializeField] private Button _refreshButton;
    [SerializeField] private TMP_Text _refreshCostText;

    [Header("Next Round")]
    [SerializeField] private Button _nextRoundButton;

    [Header("Player Tooth")]
    [SerializeField] private TMP_Text _playerToothText;

    #region 오브젝트 풀
    private ObjectPool<ProductSlotUI> _pool;
    #endregion

    #region 이벤트
    public event Action<IProduct> OnShopProductClicked;
    public event Action<IProduct> OnWeaponInventoryProductClicked;
    public event Action<IProduct> OnItemInventoryProductClicked;
    public event Action OnRefreshButtonClicked;
    public event Action OnNextRoundButtonClicked;
    #endregion

    private void Awake()
    {
        InitButtons();
    }

    //버튼 초기화
    private void InitButtons()
    {
        _refreshButton.onClick.AddListener(() => OnRefreshButtonClicked?.Invoke());
        _nextRoundButton.onClick.AddListener(() => OnNextRoundButtonClicked?.Invoke());
    }

    #region 오브젝트 풀링
    private void InitPool()
    {
        _pool = new(
            () => Instantiate(_productSlotUIPrefab),
            (slotUI) => slotUI.gameObject.SetActive(true),
            (slotUI) =>
            {
                slotUI.gameObject.SetActive(false);
                slotUI.transform.SetParent(transform, false);
            },
            (slotUI) => Destroy(slotUI.gameObject),
            false
            );
    }
    #endregion

    #region 상품 설정
    //구매 상품 설정
    public void SetShopProducts(List<IProduct> products)
    {
        UpdateProducts(products, _shopProductParent, OnShopProductClicked);
    }

    //무기 판매 상품 설정
    public void SetWeaponInventoryProducts(List<WeaponInventoryProduct> products)
    {
        UpdateProducts(products, _weaponInventoryProductParent, OnWeaponInventoryProductClicked);
    }

    //아이템 판매 상품 설정
    public void SetItemInventoryProducts(List<ItemInventoryProduct> products)
    {
        UpdateProducts(products, _itemInventoryProductParent, OnItemInventoryProductClicked);
    }

    //상품 UI 업데이트 공통 함수
    private void UpdateProducts<T>(List<T> products, Transform parent, Action<IProduct> onClicked) where T : IProduct
    {
        //기존 상품 제거
        ClearParent(parent);

        //오브젝트 풀 초기화
        if (_pool == null) InitPool();

        //새 상품 설정
        foreach (var product in products)
        {
            var slotUI = _pool.Get();
            slotUI.transform.SetParent(parent, false);
            slotUI.transform.SetAsLastSibling();
            slotUI.Init(product, onClicked);
        }
    }
    #endregion

    //부모 오브젝트의 모든 자식 제거
    private void ClearParent(Transform parent)
    {
        //뒤에서부터 제거하는 것으로 오류 회피
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i);
            if (child.TryGetComponent<ProductSlotUI>(out var slotUI))
            {
                _pool.Release(slotUI);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }

    //새로고침 코스트 설정
    public void SetRefreshCost(int cost)
    {
        _refreshCostText.text = cost.ToString();
    }
}