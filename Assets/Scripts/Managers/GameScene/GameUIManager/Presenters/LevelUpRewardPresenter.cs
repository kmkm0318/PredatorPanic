using UnityEngine;

/// <summary>
/// 레벨 업 보상 프리젠터
/// MVP 패턴에서 레벨 업 보상 UI와 플레이어 간의 상호작용을 관리
/// </summary>
public class LevelUpRewardPresenter : IPresenter
{
    private Player _player;
    private LevelUpRewardUI _levelUpRewardUI;
    private int _rewardCount = 3;

    public LevelUpRewardPresenter(Player player, LevelUpRewardUI levelUpRewardUI)
    {
        _player = player;
        _levelUpRewardUI = levelUpRewardUI;
    }

    public void Init()
    {
        RegisterEvents();
    }

    public void Reset()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        _player.OnLevelChanged += OnLevelChanged;
        _levelUpRewardUI.OnRewardSelected += HandleRewardSelected;
    }

    private void UnregisterEvents()
    {
        _player.OnLevelChanged -= OnLevelChanged;
        _levelUpRewardUI.OnRewardSelected -= HandleRewardSelected;
    }
    #endregion

    #region 이벤트 핸들러
    private void OnLevelChanged(int level)
    {
        var rewardDataList = DataManager.Instance.LevelUpRewardDataList;
        var rewardDatas = rewardDataList.LevelUpRewardDatas.GetRandomElements(_rewardCount);
        if (rewardDatas.Count == 0) return;

        _levelUpRewardUI.ShowRewards(rewardDatas);
        InputManager.Instance.EnableUIInput();
        Time.timeScale = 0f;
    }

    private void HandleRewardSelected(LevelUpRewardData data)
    {
        foreach (var effect in data.RewardEffects)
        {
            effect.ApplyEffect(_player);
        }

        _levelUpRewardUI.HideRewards();
        InputManager.Instance.EnablePlayerInput();
        Time.timeScale = 1f;
    }
    #endregion
}