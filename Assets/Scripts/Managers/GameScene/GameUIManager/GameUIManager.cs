using UnityEngine;

/// <summary>
/// 게임 UI 매니저
/// 게임 씬의 UI 요소들을 관리
/// </summary>
public class GameUIManager : MonoBehaviour
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
    #endregion

    #region 레퍼런스
    private GameManager _gameManager;
    #endregion

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        PlayerPresenter = new PlayerPresenter(gameManager.Player, _playerUI);
        PlayerPresenter.Init();

        LevelUpRewardPresenter = new LevelUpRewardPresenter(_levelUpRewardUI);
        LevelUpRewardPresenter.Init();

        ShopPresenter = new ShopPresenter(gameManager.ShopManager, _shopUI);
        ShopPresenter.Init();

        DamageTextPresenter = new DamageTextPresenter(gameManager.Player, _damageTextUI);
        DamageTextPresenter.Init();

        BossHealthListPresenter = new BossHealthListPresenter(gameManager.EnemyManager, _bossHealthListUI);
        BossHealthListPresenter.Init();

        TooltipPresenter = new TooltipPresenter(_tooltipUI, ShopPresenter);
        TooltipPresenter.Init();

        PlayerStatPresenter = new PlayerStatPresenter(gameManager.Player, _playerStatUI);
        PlayerStatPresenter.Init();

        RoundPresenter = new RoundPresenter(_gameManager, _roundUI);
        RoundPresenter.Init();

        SettingsPresenter = new SettingsPresenter(SettingsManager.Instance, _settingsUI);
        SettingsPresenter.Init();

        PausePresenter = new PausePresenter(_pauseUI, SettingsPresenter);
        PausePresenter.Init();
    }

    // 종료 시에 이벤트 해제
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
        SettingsPresenter.Reset();
        PausePresenter.Reset();
    }
}