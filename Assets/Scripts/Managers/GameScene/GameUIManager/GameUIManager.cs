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
    #endregion

    #region MVP 구조를 위한 Presenter들
    private PlayerPresenter _playerPresenter;
    public LevelUpRewardPresenter LevelUpRewardPresenter { get; private set; }
    public ShopPresenter ShopPresenter { get; private set; }
    #endregion

    #region 레퍼런스
    private GameManager _gameManager;
    #endregion

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        _playerPresenter = new PlayerPresenter(gameManager.Player, _playerUI);
        _playerPresenter.Init();

        LevelUpRewardPresenter = new LevelUpRewardPresenter(_levelUpRewardUI);
        LevelUpRewardPresenter.Init();

        ShopPresenter = new ShopPresenter(gameManager.ShopManager, _shopUI);
        ShopPresenter.Init();
    }

    // 종료 시에 이벤트 해제
    private void OnDestroy()
    {
        _playerPresenter.Reset();
        LevelUpRewardPresenter.Reset();
        ShopPresenter.Reset();
    }
}