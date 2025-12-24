using System;

/// <summary>
/// 일시정지 UI 프레젠터
/// </summary>
public class PausePresenter : IPresenter
{
    #region 레퍼런스
    private PauseUI _pauseUI;
    private SettingsPresenter _settingsPresenter;
    #endregion

    #region 이벤트
    public event Action OnResumeButtonClicked;
    #endregion

    public PausePresenter(PauseUI pauseUI, SettingsPresenter settingsPresenter)
    {
        _pauseUI = pauseUI;
        _settingsPresenter = settingsPresenter;
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

        _settingsPresenter.OnClosed += HandleOnCloseButtonClicked;
    }

    private void UnregisterEvents()
    {
        _pauseUI.OnResumeButtonClicked -= HandleOnResumeButtonClicked;
        _pauseUI.OnSettingsButtonClicked -= HandleOnSettingsButtonClicked;
        _pauseUI.OnMainMenuButtonClicked -= HandleOnMainMenuButtonClicked;

        _settingsPresenter.OnClosed -= HandleOnCloseButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnResumeButtonClicked()
    {
        OnResumeButtonClicked?.Invoke();
    }

    private void HandleOnSettingsButtonClicked()
    {
        _pauseUI.Hide(0f);
        _settingsPresenter.Show();
    }

    private void HandleOnMainMenuButtonClicked()
    {
        //TODO: MainMenu로 이동
        $"MainMenu로 이동".Log();
    }

    private void HandleOnCloseButtonClicked()
    {
        _pauseUI.Show(0f);
    }
    #endregion

    #region Show, Hide
    public void Show() => _pauseUI.Show();
    public void Hide()
    {
        //설정 UI 숨기기
        _settingsPresenter.Hide();

        //일시정지 UI 숨기기
        _pauseUI.Hide();
    }
    #endregion
}