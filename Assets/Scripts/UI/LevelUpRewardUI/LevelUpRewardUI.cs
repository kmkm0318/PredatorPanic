using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 레벨 업 UI 클래스
/// 레벨 업 시 플레이어가 보상을 선택할 수 있는 UI를 관리합니다.
/// </summary>
public class LevelUpRewardUI : ShowHideUI
{
    [Header("Prefabs & Parents")]
    [SerializeField] private LevelUpRewardSelectUI _rewardSelectUIPrefab;
    [SerializeField] private Transform _rewardSelectUIParent;

    [Header("Show Duration")]
    [SerializeField] private float _showSelectDuration = 0.2f;
    [SerializeField] private float _showDelayBetweenSelects = 0.1f;

    #region 오브젝트 풀
    private ObjectPool<LevelUpRewardSelectUI> _rewardSelectUIPool;
    private List<LevelUpRewardSelectUI> _activeRewardUIs = new();
    #endregion

    #region 이벤트
    public event Action<LevelUpRewardData> OnRewardSelected;
    #endregion

    private void Awake()
    {
        InitPool();
        Hide(0f);
    }

    private void InitPool()
    {
        _rewardSelectUIPool = new(
            () => Instantiate(_rewardSelectUIPrefab, _rewardSelectUIParent),
            (ui) => ui.gameObject.SetActive(true),
            (ui) => ui.gameObject.SetActive(false),
            (ui) => Destroy(ui.gameObject),
            false,
            3,
            10
        );
    }

    // 보상 선택 UI 설정
    public void SetRewards(List<LevelUpRewardData> rewardDatas)
    {
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            var rewardData = rewardDatas[i];
            var rewardSelectUI = _rewardSelectUIPool.Get();
            _activeRewardUIs.Add(rewardSelectUI);

            rewardSelectUI.Init(rewardData);
            rewardSelectUI.OnClicked += HandleSelectClicked;

            float duration = _showSelectDuration + i * _showDelayBetweenSelects;
            rewardSelectUI.Show(duration);
        }
    }

    // 보상 선택 시 이벤트 처리
    private void HandleSelectClicked(LevelUpRewardData data)
    {
        OnRewardSelected?.Invoke(data);
    }

    // 보상 선택 UI들 숨기기 및 이벤트 구독 해제
    public void HideRewards()
    {
        for (int i = 0; i < _activeRewardUIs.Count; i++)
        {
            _activeRewardUIs[i].OnClicked -= HandleSelectClicked;
            _rewardSelectUIPool.Release(_activeRewardUIs[i]);
        }
        _activeRewardUIs.Clear();

        Hide();
    }
}