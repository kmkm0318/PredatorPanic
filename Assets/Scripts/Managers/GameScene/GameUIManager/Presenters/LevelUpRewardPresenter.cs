using System;

/// <summary>
/// 레벨 업 보상 프리젠터
/// MVP 패턴에서 레벨 업 보상 UI와 플레이어 간의 상호작용을 관리
/// </summary>
public class LevelUpRewardPresenter : IPresenter
{
    //보상 선택지 수
    private const int REWARD_SELECT_COUNT = 3;

    #region 레퍼런스
    private LevelUpRewardUI _levelUpRewardUI;
    #endregion

    #region 이벤트
    public event Action<LevelUpRewardData> OnRewardSelected;
    #endregion

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
    public bool TryShowRewards(float luckStat = 0f, int count = REWARD_SELECT_COUNT)
    {
        // 보상 데이터 리스트 가져오기
        var rewardDataList = DataManager.Instance.LevelUpRewardDataList;

        //희귀도 가중치 데이터 가져오기
        var rarityWeightData = DataManager.Instance.RarityWeightData;

        //가중치 리스트 생성
        WeightedList<LevelUpRewardData> weightedRewardDatas = new();

        //각 가중치에 따른 보상 데이터 추가
        foreach (var rewardData in rewardDataList.LevelUpRewardDatas)
        {
            float weight = rarityWeightData.GetTotalWeight(rewardData.Rarity, luckStat);
            weightedRewardDatas.AddItem(new WeightedItem<LevelUpRewardData>(rewardData, weight));
        }

        //가중치에 따라 보상 데이터 선택
        var rewardDatas = weightedRewardDatas.GetRandomElements(count);

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