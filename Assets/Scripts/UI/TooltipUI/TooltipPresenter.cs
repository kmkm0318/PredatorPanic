/// <summary>
/// 툴팁 프레젠터 클래스
/// </summary>
public class TooltipPresenter : IPresenter
{
    #region 레퍼런스
    private TooltipUI _tooltipUI;
    #endregion

    #region 변수
    private object _currentTarget = null;
    private ITooltipProvider[] _tooltipProviders;
    #endregion

    public TooltipPresenter(TooltipUI tooltipUI, params ITooltipProvider[] tooltipProviders)
    {
        _tooltipUI = tooltipUI;

        _tooltipProviders = tooltipProviders;
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
        //등록된 모든 툴팁 제공자에서 이벤트 구독
        foreach (var provider in _tooltipProviders)
        {
            provider.OnTooltipRequested += HandleTooltipRequested;
            provider.OnTooltipRequestCanceled += HandleTooltipRequestCanceled;
        }
    }

    private void UnregisterEvents()
    {
        //등록된 모든 툴팁 제공자에서 이벤트 해제
        foreach (var provider in _tooltipProviders)
        {
            provider.OnTooltipRequested -= HandleTooltipRequested;
            provider.OnTooltipRequestCanceled -= HandleTooltipRequestCanceled;
        }
    }
    #endregion

    #region 이벤트 핸들러

    private void HandleTooltipRequested(TooltipContext context)
    {
        //현재 타겟 설정
        _currentTarget = context.Target;

        //툴팁 UI 설정
        _tooltipUI.SetName(context.Name);
        _tooltipUI.SetDescription(context.Description);
        _tooltipUI.SetIcon(context.Icon);
        _tooltipUI.SetColor(context.Color);
        _tooltipUI.SetPrice(context.Price);

        //UI 표시
        _tooltipUI.Show();
    }

    private void HandleTooltipRequestCanceled(object target)
    {
        //타겟이 null이 아니면서 현재 타겟과 요청된 타겟이 다르면 무시
        if (target != null && _currentTarget != target) return;

        //현재 타겟 초기화
        _currentTarget = null;

        //UI 숨기기
        _tooltipUI.Hide();
    }
    #endregion
}