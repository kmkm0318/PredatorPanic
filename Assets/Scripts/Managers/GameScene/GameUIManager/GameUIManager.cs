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
    #endregion

    #region MVP 구조를 위한 Presenter들
    private PlayerPresenter _playerPresenter;
    public LevelUpRewardPresenter LevelUpRewardPresenter { get; private set; }
    #endregion

    public void Init(Player player)
    {
        _playerPresenter = new PlayerPresenter(player, _playerUI);
        _playerPresenter.Init();

        LevelUpRewardPresenter = new LevelUpRewardPresenter(_levelUpRewardUI);
        LevelUpRewardPresenter.Init();
    }

    // 종료 시에 이벤트 해제
    private void OnDestroy()
    {
        _playerPresenter.Reset();
        LevelUpRewardPresenter.Reset();
    }
}