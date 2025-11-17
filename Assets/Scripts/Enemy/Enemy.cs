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

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _health = GetComponent<Health>();
    }

    public void Init(EnemyData enemyData)
    {
        _enemyData = enemyData;

        InitStats();
        InitComponents();
    }

    private void InitStats()
    {
        _enemyStats = new Stats<EnemyStatType>(_enemyData.InitialStats);
    }

    private void InitComponents()
    {
        _enemyController.Init(this, _enemyData.EnemyControllerData);

        float maxHealth = _enemyStats.GetStat(EnemyStatType.Health).FinalValue;
        float defense = _enemyStats.GetStat(EnemyStatType.Defense).FinalValue;

        _health.Init(maxHealth, defense);
        _health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        OnRelease?.Invoke(this);
    }

    public void SetTarget(Transform target)
    {
        _enemyController.SetTarget(target);
    }
}