using System;
using UnityEngine;

/// <summary>
/// 레벨 업 보상 프리젠터
/// MVP 패턴에서 레벨 업 보상 UI와 플레이어 간의 상호작용을 관리
/// </summary>
public class LevelUpRewardPresenter : IPresenter
{
    private LevelUpRewardUI _levelUpRewardUI;
    private int _rewardCount = 3;

    public event Action<LevelUpRewardData> OnRewardSelected;

    public LevelUpRewardPresenter(LevelUpRewardUI levelUpRewardUI)
    {
        _levelUpRewardUI = levelUpRewardUI;
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

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        if (_levelUpRewardUI)
        {
            _levelUpRewardUI.OnRewardSelected += HandleRewardSelected;
        }
    }

    private void UnregisterEvents()
    {
        if (_levelUpRewardUI)
        {
            _levelUpRewardUI.OnRewardSelected -= HandleRewardSelected;
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleRewardSelected(LevelUpRewardData data)
    {
        OnRewardSelected?.Invoke(data);
    }
    #endregion

    /// <summary>
    /// 보상 선택 UI 표시 함수
    /// 리워드가 없을 시 false를 반환하여 바로 LevelUp 상태를 종료할 수 있도록 함
    /// </summary>
    public bool TryShowRewards()
    {
        // 보상 데이터 리스트에서 랜덤으로 보상 선택
        var rewardDataList = DataManager.Instance.LevelUpRewardDataList;
        var rewardDatas = rewardDataList.LevelUpRewardDatas.GetRandomElements(_rewardCount);

        // 보상이 없으면 false 반환
        if (rewardDatas.Count == 0)
        {
            return false;
        }

        // 보상 선택 UI 설정 및 표시
        _levelUpRewardUI.SetRewards(rewardDatas);
        _levelUpRewardUI.Show();

        return true;
    }

    /// <summary>
    /// 보상 선택 UI 숨기기 함수
    /// LevelUp 상태 종료 시 호출
    /// </summary>
    public void HideRewards()
    {
        _levelUpRewardUI.HideRewards();
    }
}