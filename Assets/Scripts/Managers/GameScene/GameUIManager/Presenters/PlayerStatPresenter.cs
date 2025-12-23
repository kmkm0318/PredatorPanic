using System;
using System.Collections.Generic;

/// <summary>
/// 플레이어 스탯 UI를 플레이어와 연결하는 프레젠터
/// </summary>
public class PlayerStatPresenter : IPresenter
{
    #region 레퍼런스
    private Player _player;
    private PlayerStatUI _playerStatUI;
    #endregion

    private Dictionary<PlayerStatType, Action<float>> _statChangedActions = new();

    public PlayerStatPresenter(Player player, PlayerStatUI playerStatUI)
    {
        _player = player;
        _playerStatUI = playerStatUI;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        // 이벤트 구독
        RegisterEvents();

        // 초기 UI 설정
        InitUI();
    }

    private void InitUI()
    {
        // 스탯 가져오기
        var stats = _player.PlayerStats;

        // 스탯 데이터 리스트 가져오기
        var statTypeDataList = DataManager.Instance.PlayerStatTypeDataList;

        // 스탯이 없으면 패스
        if (stats == null) return;

        // 데이터 리스트가 없으면 패스
        if (statTypeDataList == null) return;

        // 플레이어 스탯 UI에 스탯 설정
        _playerStatUI.SetPlayerStats(stats, statTypeDataList);
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
        // 스탯 가져오기
        var stats = _player.PlayerStats;

        // 스탯이 없으면 패스
        if (stats == null) return;

        // 각 스탯 변경 이벤트 구독
        foreach (var statTypeObj in Enum.GetValues(typeof(PlayerStatType)))
        {
            // 스탯 타입 설정
            var statType = (PlayerStatType)statTypeObj;

            // 스탯 가져오기
            var stat = stats.GetStat(statType);

            // 스탯이 없으면 패스
            if (stat == null) continue;

            // 스탯 변경 이벤트 생성
            void onStatChanged(float value) => _playerStatUI.UpdateStatValue(statType, value);

            // 이벤트 구독
            stat.OnValueChanged += onStatChanged;

            // 딕셔너리에 저장
            _statChangedActions[statType] = onStatChanged;
        }
    }

    private void UnregisterEvents()
    {
        // 스탯 가져오기
        var stats = _player.PlayerStats;

        // 스탯이 없으면 패스
        if (stats == null) return;

        // 각 스탯 변경 이벤트 해제
        foreach (var pair in _statChangedActions)
        {
            var statType = pair.Key;
            var action = pair.Value;

            // 스탯 가져오기
            var stat = stats.GetStat(statType);

            // 스탯이 없으면 패스
            if (stat == null) continue;

            // 이벤트 해제
            stat.OnValueChanged -= action;
        }

        // 딕셔너리 초기화
        _statChangedActions.Clear();
    }
    #endregion

    #region Show, Hide
    public void Show()
    {
        _playerStatUI.Show();
    }

    public void Hide()
    {
        _playerStatUI.Hide();
    }
    #endregion
}

