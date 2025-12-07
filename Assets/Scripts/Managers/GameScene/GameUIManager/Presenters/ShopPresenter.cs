using System;
using System.Collections.Generic;

/// <summary>
/// 상점 UI 프리젠터 클래스
/// </summary>
public class ShopPresenter : IPresenter
{
    #region 레퍼런스
    private ShopManager _shopManager;
    private ShopUI _shopUI;
    #endregion

    #region 이벤트
    public event Action OnNextRoundButtonClicked;
    #endregion

    //생성자에서 레퍼런스 변수 주입
    public ShopPresenter(ShopManager shopManager, ShopUI shopUI)
    {
        _shopManager = shopManager;
        _shopUI = shopUI;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        RegisterEvents();
        InitUI();
    }

    //초기 UI 설정
    private void InitUI()
    {
        _shopUI.SetRefreshCost(_shopManager.CurrentRefreshCost);
        _shopUI.SetShopProducts(_shopManager.ShopProducts);
        _shopUI.SetWeaponInventoryProducts(_shopManager.WeaponInventoryProducts);
        _shopUI.SetItemInventoryProducts(_shopManager.ItemInventoryProducts);
    }

    public void Reset()
    {
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        if (_shopManager)
        {
            _shopManager.OnCurrentRefreshCostChanged += OnCurrentRefreshCostChanged;
            _shopManager.OnShopProductsUpdated += OnShopProductsUpdated;
            _shopManager.OnWeaponInventoryProductsUpdated += OnWeaponInventoryProductsUpdated;
            _shopManager.OnItemInventoryProductsUpdated += OnItemInventoryProductsUpdated;
        }

        if (_shopUI)
        {
            _shopUI.OnShopProductClicked += OnShopProductClicked;
            _shopUI.OnWeaponInventoryProductClicked += OnWeaponInventoryProductClicked;
            _shopUI.OnItemInventoryProductClicked += OnItemInventoryProductClicked;
            _shopUI.OnRefreshButtonClicked += OnRefreshButtonClicked;
            _shopUI.OnNextRoundButtonClicked += OnNextRoundButtonClick;
        }
    }

    private void UnregisterEvents()
    {
        if (_shopManager)
        {
            _shopManager.OnShopProductsUpdated += OnShopProductsUpdated;
            _shopManager.OnWeaponInventoryProductsUpdated += OnWeaponInventoryProductsUpdated;
            _shopManager.OnItemInventoryProductsUpdated += OnItemInventoryProductsUpdated;
        }

        if (_shopUI)
        {
            _shopUI.OnShopProductClicked -= OnShopProductClicked;
            _shopUI.OnWeaponInventoryProductClicked -= OnWeaponInventoryProductClicked;
            _shopUI.OnItemInventoryProductClicked -= OnItemInventoryProductClicked;
            _shopUI.OnRefreshButtonClicked -= OnRefreshButtonClicked;
            _shopUI.OnNextRoundButtonClicked -= OnNextRoundButtonClick;
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void OnCurrentRefreshCostChanged(int cost)
    {
        _shopUI.SetRefreshCost(cost);
    }

    private void OnShopProductsUpdated(List<IProduct> shopProducts)
    {
        _shopUI.SetShopProducts(shopProducts);
    }

    private void OnWeaponInventoryProductsUpdated(List<WeaponInventoryProduct> weaponInventoryProducts)
    {
        _shopUI.SetWeaponInventoryProducts(weaponInventoryProducts);
    }

    private void OnItemInventoryProductsUpdated(List<ItemInventoryProduct> itemInventoryProducts)
    {
        _shopUI.SetItemInventoryProducts(itemInventoryProducts);
    }

    private void OnShopProductClicked(IProduct product)
    {
        //상품 구매 시도
        _shopManager.TryBuyProduct(product);
    }

    private void OnWeaponInventoryProductClicked(IProduct product)
    {
        //무기 상품이 아닐 시 무시
        if (product is not WeaponInventoryProduct weaponProduct) return;

        //상품 판매 시도
        _shopManager.TrySellWeaponProduct(weaponProduct);
    }

    private void OnItemInventoryProductClicked(IProduct product)
    {
        //아이템 상품이 아닐 시 무시
        if (product is not ItemInventoryProduct itemProduct) return;

        //상품 판매 시도
        _shopManager.TrySellItemProduct(itemProduct);
    }

    //상점 새로고침 버튼 클릭 핸들러
    private void OnRefreshButtonClicked()
    {
        //상점 상품 새로고침 시도
        _shopManager.TryRefreshShopProducts();
    }

    //다음 라운드 버튼 클릭 핸들러
    private void OnNextRoundButtonClick()
    {
        OnNextRoundButtonClicked?.Invoke();
    }
    #endregion

    #region Show/Hide 함수
    /// <summary>
    /// 상점 UI 표시 함수
    /// ShopState진입 시 호출합니다
    /// </summary>
    public void ShowShopUI()
    {
        //상점 매니저의 모든 상품 갱신

        //판매 상품 갱신. 최초 갱신
        _shopManager.TryRefreshShopProducts(true);

        //UI 표시
        _shopUI.Show();
    }

    /// <summary>
    /// 상점 UI 숨기기 함수
    /// ShopState 종료 시 호출합니다
    /// </summary>
    public void HideShopUI()
    {
        _shopUI.Hide();
    }
    #endregion
}