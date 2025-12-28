using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 UI 매니저
/// 게임 씬의 UI 요소들을 관리
/// </summary>
public class GameUIManager : MonoBehaviour, ICancelableManager
{
    #region 씬 내 UI
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private LevelUpRewardUI _levelUpRewardUI;
    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private DamageTextUI _damageTextUI;
    [SerializeField] private BossHealthListUI _bossHealthListUI;
    [SerializeField] private TooltipUI _tooltipUI;
    [SerializeField] private PlayerStatUI _playerStatUI;
    [SerializeField] private RoundUI _roundUI;
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private ConfirmPopupUI _confirmPopupUI;
    [SerializeField] private GameResultUI _gameResultUI;
    #endregion

    #region MVP 구조를 위한 Presenter들
    public PlayerPresenter PlayerPresenter { get; private set; }
    public LevelUpRewardPresenter LevelUpRewardPresenter { get; private set; }
    public ShopPresenter ShopPresenter { get; private set; }
    public DamageTextPresenter DamageTextPresenter { get; private set; }
    public BossHealthListPresenter BossHealthListPresenter { get; private set; }
    public TooltipPresenter TooltipPresenter { get; private set; }
    public PlayerStatPresenter PlayerStatPresenter { get; private set; }
    public RoundPresenter RoundPresenter { get; private set; }
    public SettingsPresenter SettingsPresenter { get; private set; }
    public PausePresenter PausePresenter { get; private set; }
    public ConfirmPopupPresenter ConfirmPopupPresenter { get; private set; }
    public GameResultPresenter GameResultPresenter { get; private set; }
    #endregion

    #region 레퍼런스
    private GameManager _gameManager;
    #endregion

    #region 변수
    private Stack<ICancelable> _cancelableStack = new();
    #endregion

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        RegisterEvents();

        InitPresenter();
    }

    private void InitPresenter()
    {
        PlayerPresenter = new PlayerPresenter(_gameManager.Player, _playerUI);
        PlayerPresenter.Init();

        LevelUpRewardPresenter = new LevelUpRewardPresenter(_levelUpRewardUI);
        LevelUpRewardPresenter.Init();

        ShopPresenter = new ShopPresenter(_gameManager.ShopManager, _shopUI);
        ShopPresenter.Init();

        DamageTextPresenter = new DamageTextPresenter(_gameManager.Player, _damageTextUI);
        DamageTextPresenter.Init();

        BossHealthListPresenter = new BossHealthListPresenter(_gameManager.EnemyManager, _bossHealthListUI);
        BossHealthListPresenter.Init();

        TooltipPresenter = new TooltipPresenter(_tooltipUI, ShopPresenter);
        TooltipPresenter.Init();

        PlayerStatPresenter = new PlayerStatPresenter(_gameManager.Player, _playerStatUI);
        PlayerStatPresenter.Init();

        RoundPresenter = new RoundPresenter(_gameManager, _roundUI);
        RoundPresenter.Init();

        ConfirmPopupPresenter = new ConfirmPopupPresenter(_confirmPopupUI, this);
        ConfirmPopupPresenter.Init();

        SettingsPresenter = new SettingsPresenter(SettingsManager.Instance, _settingsUI, ConfirmPopupPresenter, this);
        SettingsPresenter.Init();

        PausePresenter = new PausePresenter(_pauseUI, SettingsPresenter, this);
        PausePresenter.Init();

        GameResultPresenter = new GameResultPresenter(_gameResultUI);
        GameResultPresenter.Init();
    }

    private void OnDestroy()
    {
        PlayerPresenter.Reset();
        LevelUpRewardPresenter.Reset();
        ShopPresenter.Reset();
        DamageTextPresenter.Reset();
        BossHealthListPresenter.Reset();
        TooltipPresenter.Reset();
        PlayerStatPresenter.Reset();
        RoundPresenter.Reset();
        ConfirmPopupPresenter.Reset();
        SettingsPresenter.Reset();
        PausePresenter.Reset();
        GameResultPresenter.Reset();

        UnregisterEvents();
    }

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        //인풋 액션 가져오기
        var inputActions = InputManager.Instance.PlayerInputActions;

        //Cancel 이벤트 구독
        inputActions.UI.Cancel.performed += HandleOnCancelPerformed;
    }

    private void UnregisterEvents()
    {
        //인풋 액션 가져오기
        var inputActions = InputManager.Instance.PlayerInputActions;

        //Cancel 이벤트 해제
        inputActions.UI.Cancel.performed -= HandleOnCancelPerformed;
    }

    private void HandleOnCancelPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_cancelableStack.Count > 0)
        {
            //가장 위에 있는 ICancelable 객체의 Cancel 메서드 호출
            var topCancelable = _cancelableStack.Peek();
            topCancelable.Cancel();
        }
    }
    #endregion

    #region ICancelable 추가 및 제거
    public void PushCancelable(ICancelable cancelable)
    {
        _cancelableStack.Push(cancelable);
    }

    public void PopCancelable(ICancelable cancelable)
    {
        if (_cancelableStack.Count > 0 && _cancelableStack.Peek() == cancelable)
        {
            _cancelableStack.Pop();
        }
    }
    #endregion
}