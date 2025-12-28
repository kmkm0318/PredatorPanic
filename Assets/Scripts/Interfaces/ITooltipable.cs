using System;

/// <summary>
/// 툴팁이 가능한 오브젝트 인터페이스
/// </summary>
public interface ITooltipProvider
{
    public event Action<TooltipContext> OnTooltipRequested;
    public event Action<object> OnTooltipRequestCanceled;
}