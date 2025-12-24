/// <summary>
/// 툴팁 프레젠터 클래스
/// </summary>
public class TooltipPresenter : IPresenter
{
    #region 레퍼런스
    private TooltipUI _tooltipUI;
    private ShopPresenter _shopPresenter;
    #endregion

    #region 변수
    private object _currentTooltipTarget = null;
    #endregion

    public TooltipPresenter(TooltipUI tooltipUI, ShopPresenter shopPresenter)
    {
        _tooltipUI = tooltipUI;
        _shopPresenter = shopPresenter;
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
        _shopPresenter.OnProductPointerEntered += HandleProductPointerEntered;
        _shopPresenter.OnProductPointerExited += HandleProductPointerExited;
        _shopPresenter.OnShopUpdated += HandleShopUpdated;
    }

    private void UnregisterEvents()
    {
        _shopPresenter.OnProductPointerEntered -= HandleProductPointerEntered;
        _shopPresenter.OnProductPointerExited -= HandleProductPointerExited;
        _shopPresenter.OnShopUpdated -= HandleShopUpdated;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleProductPointerEntered(IProduct product)
    {
        _currentTooltipTarget = product;
        _tooltipUI.Show(product);
    }

    private void HandleProductPointerExited(IProduct product)
    {
        if (_currentTooltipTarget == product)
        {
            _currentTooltipTarget = null;
            _tooltipUI.Hide();
        }
    }

    private void HandleShopUpdated()
    {
        _currentTooltipTarget = null;
        _tooltipUI.Hide();
    }
    #endregion
}