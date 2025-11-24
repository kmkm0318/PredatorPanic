using System;

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
    }

    public void Reset()
    {
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        _shopUI.OnShopProductClicked += OnShopProductClicked;
        _shopUI.OnWeaponInventoryProductClicked += OnWeaponInventoryProductClicked;
        _shopUI.OnItemInventoryProductClicked += OnItemInventoryProductClicked;
        _shopUI.OnRefreshButtonClicked += OnRefreshButtonClicked;
        _shopUI.OnNextRoundButtonClicked += OnNextRoundButtonClick;
    }

    private void UnregisterEvents()
    {
        _shopUI.OnShopProductClicked -= OnShopProductClicked;
        _shopUI.OnWeaponInventoryProductClicked -= OnWeaponInventoryProductClicked;
        _shopUI.OnItemInventoryProductClicked -= OnItemInventoryProductClicked;
        _shopUI.OnRefreshButtonClicked -= OnRefreshButtonClicked;
        _shopUI.OnNextRoundButtonClicked -= OnNextRoundButtonClick;
    }
    #endregion

    #region 이벤트 핸들러
    private void OnShopProductClicked(IProduct product)
    {
        //상품 구매 시도
        bool success = _shopManager.TryBuyProduct(product);

        //구매 성공 시 UI 갱신
        if (success)
        {
            _shopManager.RemoveShopProduct(product);
            _shopUI.SetShopProducts(_shopManager.BuyProducts);
        }
    }

    private void OnWeaponInventoryProductClicked(IProduct product)
    {
        //무기 상품이 아닐 시 무시
        if (product is not WeaponInventoryProduct weaponProduct) return;

        //상품 판매 시도
        bool success = _shopManager.TrySellWeaponProduct(weaponProduct);

        //판매 성공 시 UI 갱신
        if (success)
        {
            _shopManager.RefreshWeaponInventoryProducts();
            _shopUI.SetWeaponInventoryProducts(_shopManager.WeaponSellProducts);
        }
    }

    private void OnItemInventoryProductClicked(IProduct product)
    {
        //아이템 상품이 아닐 시 무시
        if (product is not ItemInventoryProduct itemProduct) return;

        //상품 판매 시도
        bool success = _shopManager.TrySellItemProduct(itemProduct);

        //판매 성공 시 UI 갱신
        if (success)
        {
            _shopManager.RefreshItemInventoryProducts();
            _shopUI.SetItemInventoryProducts(_shopManager.ItemSellProducts);
        }
    }

    //상점 새로고침 버튼 클릭 핸들러
    private void OnRefreshButtonClicked()
    {
        _shopManager.TryRefreshShopProducts();

        _shopUI.SetShopProducts(_shopManager.BuyProducts);

        _shopUI.SetRefreshCost(_shopManager.CurrentRefreshCost);
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
        _shopManager.RefreshAllProduct();

        _shopUI.SetShopProducts(_shopManager.BuyProducts);
        _shopUI.SetWeaponInventoryProducts(_shopManager.WeaponSellProducts);
        _shopUI.SetItemInventoryProducts(_shopManager.ItemSellProducts);

        _shopUI.SetRefreshCost(_shopManager.CurrentRefreshCost);

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