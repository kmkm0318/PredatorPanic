/// <summary>
/// 라운드 프리젠터 클래스
/// </summary>
public class RoundPresenter : IPresenter
{
    #region 레퍼런스
    private GameManager _gameManager;
    private RoundUI _roundUI;
    #endregion

    public RoundPresenter(GameManager gameManager, RoundUI roundUI)
    {
        _gameManager = gameManager;
        _roundUI = roundUI;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        // 이벤트 등록
        RegisterEvents();

        // 초기 UI 설정
        InitUI();
    }

    private void InitUI()
    {
        // 라운드 설정
        _roundUI.ShowRoundText(false);

        // 라운드 시간 설정
        _roundUI.ShowRoundTimerText(false);
    }

    public void Reset()
    {
        // 이벤트 해제
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        _gameManager.OnCurrentRoundChanged += _roundUI.SetRoundText;
        _gameManager.OnRoundTimerChanged += _roundUI.SetRoundTimerText;
    }

    private void UnregisterEvents()
    {
        _gameManager.OnCurrentRoundChanged -= _roundUI.SetRoundText;
        _gameManager.OnRoundTimerChanged -= _roundUI.SetRoundTimerText;
    }
    #endregion

    #region Show Text
    public void ShowRoundText(bool isShow)
    {
        _roundUI.ShowRoundText(isShow);
    }

    public void ShowRoundTimerText(bool isShow)
    {
        _roundUI.ShowRoundTimerText(isShow);
    }
    #endregion
}