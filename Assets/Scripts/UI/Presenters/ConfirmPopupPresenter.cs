using System;

/// <summary>
/// 확인 팝업 프레젠터
/// </summary>
public class ConfirmPopupPresenter : IPresenter, ICancelable
{
    #region 레퍼런스
    private ConfirmPopupUI _confirmPopupUI;
    private ICancelableManager _cancelableManager;
    #endregion

    #region 이벤트
    public event Action OnConfirmed;
    public event Action OnCancelled;
    #endregion

    public ConfirmPopupPresenter(ConfirmPopupUI confirmPopupUI, ICancelableManager cancelableManager)
    {
        _confirmPopupUI = confirmPopupUI;
        _cancelableManager = cancelableManager;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        //이벤트 등록
        _confirmPopupUI.OnConfirmed += HandleOnConfirmed;
        _confirmPopupUI.OnCancelled += HandleOnCancelled;
    }

    public void Reset()
    {
        //이벤트 해제
        _confirmPopupUI.OnConfirmed -= HandleOnConfirmed;
        _confirmPopupUI.OnCancelled -= HandleOnCancelled;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnConfirmed()
    {
        OnConfirmed?.Invoke();
        Hide();
    }

    private void HandleOnCancelled()
    {
        OnCancelled?.Invoke();
        Hide();
    }
    #endregion

    #region Show, Hide
    public void Show(string message)
    {
        //메세지 설정
        _confirmPopupUI.SetMessage(message);

        //UI 표시
        _confirmPopupUI.Show(0f);

        //Cancelable 매니저에 추가
        _cancelableManager.PushCancelable(this);
    }

    public void Hide()
    {
        //UI 숨기기
        _confirmPopupUI.Hide(0f);

        //Cancelable 매니저에서 제거
        _cancelableManager.PopCancelable(this);
    }
    #endregion

    public void Cancel()
    {
        //취소 버튼을 누른 것으로 처리
        HandleOnCancelled();
    }
}