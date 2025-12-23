using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 플레이어 스탯 UI 클래스
/// </summary>
public class PlayerStatUI : ShowHideUI
{
    [Header("UI Components")]
    [SerializeField] private PlayerStatItem _playerStatItemPrefab;
    [SerializeField] private Transform _statItemParent;

    #region 오브젝트 풀
    private ObjectPool<PlayerStatItem> _playerStatItemPool;
    private Dictionary<PlayerStatType, PlayerStatItem> _activeStatItems = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool()
    {
        _playerStatItemPool = new(
            () => Instantiate(_playerStatItemPrefab, _statItemParent),
            item => item.gameObject.SetActive(true),
            item => item.gameObject.SetActive(false),
            item => Destroy(item.gameObject));
    }
    #endregion

    #region 스탯 설정
    /// <summary>
    /// 플레이어 스탯 설정 함수
    /// </summary>
    public void SetPlayerStats(Stats<PlayerStatType> stats, PlayerStatTypeDataList statTypeDataList)
    {
        //스탯이 없으면 패스
        if (stats == null) return;

        //데이터 리스트가 없으면 패스
        if (statTypeDataList == null) return;

        //오브젝트 풀이 초기화되지 않았으면 초기화
        if (_playerStatItemPool == null) InitPool();

        //기존 스탯 클리어
        ClearStats();

        //새로운 스탯 설정
        foreach (var stat in Enum.GetValues(typeof(PlayerStatType)))
        {
            //스탯 타입 가져오기
            var statType = (PlayerStatType)stat;

            //스탯 값 가져오기
            var statValue = stats.GetStat(statType).FinalValue;

            //스탯 데이터 가져오기
            var statTypeData = statTypeDataList.GetData(statType);

            if (statTypeData != null)
            {
                //스탯 아이템 가져오기
                var statItem = _playerStatItemPool.Get();

                //스탯 이름 및 값 설정
                statItem.SetStat(statTypeData.StatName, statValue.ToString("0.##"));

                //활성화된 스탯 아이템 딕셔너리에 추가
                _activeStatItems[statType] = statItem;
            }
        }
    }

    /// <summary>
    /// 스탯 값 업데이트 함수
    /// </summary>
    public void UpdateStatValue(PlayerStatType statType, float newValue)
    {
        //활성화된 스탯 아이템에서 해당 스탯 타입이 있는지 확인
        if (_activeStatItems.TryGetValue(statType, out var statItem))
        {
            //스탯 값 업데이트
            statItem.SetStatValue(newValue.ToString("0.##"));
        }
    }

    /// <summary>
    /// 스탯 클리어 함수
    /// </summary>
    private void ClearStats()
    {
        //모든 자식 아이템을 풀에 반환
        while (_statItemParent.childCount > 0)
        {
            var item = _statItemParent.GetChild(0).GetComponent<PlayerStatItem>();
            _playerStatItemPool.Release(item);
        }

        //활성화된 스탯 아이템 딕셔너리 클리어
        _activeStatItems.Clear();
    }
    #endregion
}