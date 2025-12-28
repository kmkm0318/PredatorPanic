using System;

/// <summary>
/// 메인 메뉴의 프레젠터 클래스
/// </summary>
public class MainMenuPresenter : IPresenter
{
    #region 레퍼런스
    private MainMenuManager _mainMenuManager;
    private MainMenuUI _mainMenuUI;
    #endregion

    public event Action OnStartButtonClicked;
    public event Action OnEvolutionButtonClicked;
    public event Action OnSettingsButtonClicked;
    public event Action OnExitButtonClicked;

    public MainMenuPresenter(MainMenuManager mainMenuManager, MainMenuUI mainMenuUI)
    {
        _mainMenuManager = mainMenuManager;
        _mainMenuUI = mainMenuUI;
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

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        _mainMenuUI.OnStartButtonClicked += HandleStartButtonClicked;
        _mainMenuUI.OnEvolutionButtonClicked += HandleEvolutionButtonClicked;
        _mainMenuUI.OnSettingsButtonClicked += HandleSettingsButtonClicked;
        _mainMenuUI.OnExitButtonClicked += HandleExitButtonClicked;
    }

    private void UnregisterEvents()
    {
        _mainMenuUI.OnStartButtonClicked -= HandleStartButtonClicked;
        _mainMenuUI.OnEvolutionButtonClicked -= HandleEvolutionButtonClicked;
        _mainMenuUI.OnSettingsButtonClicked -= HandleSettingsButtonClicked;
        _mainMenuUI.OnExitButtonClicked -= HandleExitButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleStartButtonClicked()
    {
        OnStartButtonClicked?.Invoke();
    }

    private void HandleEvolutionButtonClicked()
    {
        OnEvolutionButtonClicked?.Invoke();
    }

    private void HandleSettingsButtonClicked()
    {
        OnSettingsButtonClicked?.Invoke();
    }

    private void HandleExitButtonClicked()
    {
        OnExitButtonClicked?.Invoke();
    }
    #endregion
}