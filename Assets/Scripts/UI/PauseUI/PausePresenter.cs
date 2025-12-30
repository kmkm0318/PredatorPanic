using System;

/// <summary>
/// 일시정지 UI 프레젠터
/// </summary>
public class PausePresenter : IPresenter, ICancelable
{
    #region 레퍼런스
    private PauseUI _pauseUI;
    private SettingsPresenter _settingsPresenter;
    private ICancelableManager _cancelableManager;
    #endregion

    #region 이벤트
    public event Action OnResumeRequested;
    public event Action OnMainMenuRequested;
    #endregion

    public PausePresenter(PauseUI pauseUI, SettingsPresenter settingsPresenter, ICancelableManager cancelableManager)
    {
        _pauseUI = pauseUI;
        _settingsPresenter = settingsPresenter;
        _cancelableManager = cancelableManager;
    }

    #region 초기화, 리셋
    public void Init()
    {
        //이벤트 등록
        RegisterEvents();

        //UI 초기화
        _pauseUI.Init();
    }

    public void Reset()
    {
        //이벤트 해제
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        _pauseUI.OnResumeButtonClicked += HandleOnResumeButtonClicked;
        _pauseUI.OnSettingsButtonClicked += HandleOnSettingsButtonClicked;
        _pauseUI.OnMainMenuButtonClicked += HandleOnMainMenuButtonClicked;
    }

    private void UnregisterEvents()
    {
        _pauseUI.OnResumeButtonClicked -= HandleOnResumeButtonClicked;
        _pauseUI.OnSettingsButtonClicked -= HandleOnSettingsButtonClicked;
        _pauseUI.OnMainMenuButtonClicked -= HandleOnMainMenuButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnResumeButtonClicked()
    {
        //닫기
        Hide();

        //계속하기 요청 이벤트 호출
        OnResumeRequested?.Invoke();
    }

    private void HandleOnSettingsButtonClicked()
    {
        //설정 UI 표시
        _settingsPresenter.Show();

        //설정 UI가 닫히면 다시 열리도록 이벤트 구독
        _settingsPresenter.OnClosed += HandleOnClosed;

        //일시정지 UI 즉시 숨기기
        Hide(0f);
    }

    private void HandleOnClosed()
    {
        //이벤트 해제
        _settingsPresenter.OnClosed -= HandleOnClosed;

        //일시정지 UI 즉시 다시 표시
        Show(0f);
    }

    private void HandleOnMainMenuButtonClicked()
    {
        //일시정지 UI 즉시 숨기기
        Hide();

        //메인 메뉴 요청 이벤트 호출
        OnMainMenuRequested?.Invoke();
    }
    #endregion

    #region Show, Hide
    public void Show(float duration = 0.5f)
    {
        //일시정지 UI 표시
        _pauseUI.Show(duration);

        //취소 가능한 항목으로 등록
        _cancelableManager.PushCancelable(this);
    }

    public void Hide(float duration = 0.5f)
    {
        //일시정지 UI 숨기기
        _pauseUI.Hide(duration);

        //취소 가능한 항목에서 제거
        _cancelableManager.PopCancelable(this);
    }

    public void Cancel()
    {
        //계속하기 버튼을 누른 것으로 처리
        HandleOnResumeButtonClicked();
    }
    #endregion
}