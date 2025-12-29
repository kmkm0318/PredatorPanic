using System;

/// <summary>
/// 메인 메뉴의 프레젠터 클래스
/// </summary>
public class MainMenuPresenter : IPresenter
{
    #region 레퍼런스
    private MainMenuManager _mainMenuManager;
    private MainMenuUI _mainMenuUI;
    private StartPresenter _startPresenter;
    private EvolutionPresenter _evolutionPresenter;
    private SettingsPresenter _settingsPresenter;
    #endregion

    public event Action OnExitButtonClicked;

    public MainMenuPresenter(MainMenuManager mainMenuManager, MainMenuUI mainMenuUI, StartPresenter startPresenter, EvolutionPresenter evolutionPresenter, SettingsPresenter settingsPresenter)
    {
        _mainMenuManager = mainMenuManager;
        _mainMenuUI = mainMenuUI;
        _startPresenter = startPresenter;
        _evolutionPresenter = evolutionPresenter;
        _settingsPresenter = settingsPresenter;
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
        //시작 UI 닫힘 이벤트 구독
        _startPresenter.OnClosed += HandleOnStartUIClosed;

        //메인 메뉴 숨기기
        _mainMenuUI.Hide(0f);

        //시작 UI 표시
        _startPresenter.Show();
    }

    private void HandleOnStartUIClosed()
    {
        //시작 UI 닫힘 이벤트 해제
        _startPresenter.OnClosed -= HandleOnStartUIClosed;

        //메인 메뉴 다시 표시
        _mainMenuUI.Show(0f);
    }

    private void HandleEvolutionButtonClicked()
    {
        //진화 UI 닫힘 이벤트 구독
        _evolutionPresenter.OnClosed += HandleOnEvolutionUIClosed;

        //메인 메뉴 숨기기
        _mainMenuUI.Hide(0f);

        //진화 UI 표시
        _evolutionPresenter.Show();
    }

    private void HandleOnEvolutionUIClosed()
    {
        //진화 UI 닫힘 이벤트 해제
        _evolutionPresenter.OnClosed -= HandleOnEvolutionUIClosed;

        //메인 메뉴 다시 표시
        _mainMenuUI.Show(0f);
    }

    private void HandleSettingsButtonClicked()
    {
        //설정 UI 닫힘 이벤트 구독
        _settingsPresenter.OnClosed += HandleOnSettingsUIClosed;

        //메인 메뉴 숨기기
        _mainMenuUI.Hide(0f);

        //설정 UI 표시
        _settingsPresenter.Show();
    }

    private void HandleOnSettingsUIClosed()
    {
        //설정 UI 닫힘 이벤트 해제
        _settingsPresenter.OnClosed -= HandleOnSettingsUIClosed;

        //메인 메뉴 다시 표시
        _mainMenuUI.Show(0f);
    }

    private void HandleExitButtonClicked()
    {
        //종료 버튼 클릭 이벤트 호출
        OnExitButtonClicked?.Invoke();
    }
    #endregion
}