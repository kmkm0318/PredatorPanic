using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 공격 클래스
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    #region 레퍼런스
    private Enemy _enemy;
    private Player _player;
    private IndicatedAttackManager _indicatedAttackManager;
    #endregion

    #region 데이터 리스트
    private List<EnemyAttackPatternData> _attackPatterns;
    #endregion

    #region 패턴 타이머
    private Dictionary<EnemyAttackPatternData, float> _patternTimes = new();
    #endregion

    public void Init(Enemy enemy, IndicatedAttackManager indicatedAttackManager)
    {
        //적 할당
        _enemy = enemy;

        //패턴 리스트 할당
        _attackPatterns = enemy.EnemyData.AttackPatterns;

        //매니저 할당
        _indicatedAttackManager = indicatedAttackManager;

        //리스트가 비었을 시 비활성화
        if (_attackPatterns == null || _attackPatterns.Count == 0) enabled = false;

        //패턴 시간 초기화
        InitPatternTimes();
    }

    private void InitPatternTimes()
    {
        //현재 시간 가져오기
        float currentTime = Time.time;

        //패턴 별 시간 초기화
        _patternTimes.Clear();

        foreach (var pattern in _attackPatterns)
        {
            _patternTimes.Add(pattern, currentTime);
        }
    }

    public void SetTarget(Player player)
    {
        //플레이어 할당
        _player = player;
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        //목표 플레이어가 없으면 패스
        if (_player == null) return;

        //현재 시간 가져오기
        float currentTime = Time.time;

        //데미지 가져오기
        float damage = _enemy.EnemyStats.GetStat(EnemyStatType.Damage).FinalValue;

        //각 패턴별로 공격 시도
        foreach (var pattern in _attackPatterns)
        {
            //쿨타임이 지나지 않았으면 패스
            if (currentTime < _patternTimes[pattern]) continue;

            //공격 위치들 가져오기
            var attackPositions = pattern.GetAttackPositions(_enemy, _player);

            //각 위치에 공격 소환
            foreach (var pos in attackPositions)
            {
                //인디케이터 소환
                _indicatedAttackManager.SpawnIndicatedAttack(pattern.IndicatedAttackData, pos, pattern.AttackRadius, pattern.AttackDelay, damage);
            }

            //패턴 시간 초기화
            _patternTimes[pattern] = currentTime + pattern.AttackCooldown;
        }
    }
}