using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    #region 데이터
    private EnemyData _enemyData;
    #endregion

    #region 컴포넌트
    private EnemyController _enemyController;
    private Health _health;
    #endregion

    #region 적 스탯
    private Stats<EnemyStatType> _enemyStats;
    public Stats<EnemyStatType> EnemyStats => _enemyStats;
    #endregion

    #region 이벤트
    public event Action<Enemy> OnRelease;
    #endregion

    public void Init(EnemyData enemyData)
    {
        _enemyData = enemyData;

        InitStats();
        InitComponents();
    }

    private void InitStats()
    {
        _enemyStats = new Stats<EnemyStatType>(_enemyData.InitialStats.Cast<IStatEntity<EnemyStatType>>().ToList());
    }

    private void InitComponents()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyController.Init(this, _enemyData.EnemyControllerData);

        _health = GetComponent<Health>();
        _health.Init(_enemyStats.GetStat(EnemyStatType.Health).FinalValue);
        _health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        StartCoroutine(DelayedRelease(0.5f));
    }

    private IEnumerator DelayedRelease(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnRelease?.Invoke(this);
    }

    public void SetTarget(Transform target)
    {
        _enemyController.SetTarget(target);
    }
}