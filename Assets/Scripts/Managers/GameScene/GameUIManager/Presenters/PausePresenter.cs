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
    public event Action OnPuaseUIClosed;
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
    }

    private void HandleOnSettingsButtonClicked()
    {
        //설정 UI 표시
        _settingsPresenter.Show();
    }

    private void HandleOnMainMenuButtonClicked()
    {
        //TODO: MainMenu로 이동
        $"MainMenu로 이동".Log();
    }
    #endregion

    #region Show, Hide
    public void Show()
    {
        //일시정지 UI 표시
        _pauseUI.Show();

        //취소 가능한 항목으로 등록
        _cancelableManager.PushCancelable(this);
    }

    public void Hide()
    {
        //일시정지 UI 숨기기
        _pauseUI.Hide();

        //취소 가능한 항목에서 제거
        _cancelableManager.PopCancelable(this);

        //닫힘 이벤트 호출
        OnPuaseUIClosed?.Invoke();
    }

    public void Cancel()
    {
        //닫기 처리
        Hide();
    }
    #endregion
}