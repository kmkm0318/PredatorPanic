using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyHealth))]
public class Enemy : MonoBehaviour
{
    #region 데이터
    private EnemyData _enemyData;
    #endregion

    #region 컴포넌트
    private EnemyController _enemyController;
    private EnemyHealth _enemyHealth;
    #endregion

    #region 이벤트
    public event Action<Enemy> OnRelease;
    #endregion

    public void Init(EnemyData enemyData)
    {
        _enemyData = enemyData;

        InitComponents();
    }

    private void InitComponents()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyController.Init(_enemyData.EnemyControllerData);

        _enemyHealth = GetComponent<EnemyHealth>();
        _enemyHealth.Init(_enemyData);
        _enemyHealth.OnDeath += OnDeath;
    }

    private void OnDeath(Vector3 vector)
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