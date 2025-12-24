/// <summary>
/// 취소 가능한 매니저 인터페이스
/// </summary>
public interface ICancelableManager
{
    void PushCancelable(ICancelable cancelable);
    void PopCancelable(ICancelable cancelable);
}