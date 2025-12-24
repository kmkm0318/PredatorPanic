using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 보스 체력 리스트 UI 클래스
/// </summary>
public class BossHealthListUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private BossHealthUI _bossHealthUIPrefab;
    [SerializeField] private Transform _bossHealthUIParent;

    #region 오브젝트 풀
    private ObjectPool<BossHealthUI> _healthUIPool;
    #endregion

    #region 변수
    private Dictionary<Enemy, BossHealthUI> _healthUIDict = new();
    #endregion

    public void Init()
    {
        InitPool();
    }

    #region 오브젝트 풀링
    private void InitPool()
    {
        _healthUIPool = new ObjectPool<BossHealthUI>(
            () => Instantiate(_bossHealthUIPrefab, _bossHealthUIParent),
            (ui) =>
            {
                ui.gameObject.SetActive(true);
                ui.transform.SetAsLastSibling();
            },
            (ui) => ui.gameObject.SetActive(false),
            (ui) => Destroy(ui.gameObject),
            false
        );
    }
    #endregion

    #region UI 추가, 삭제, 업데이트
    /// <summary>
    /// 보스 체력 UI 추가
    /// </summary>
    public void AddBossHealthUI(Enemy bossEnemy)
    {
        //이미 존재하는 UI일 시 패스
        if (_healthUIDict.ContainsKey(bossEnemy)) return;

        //UI 초기화
        string bossName = bossEnemy.EnemyData.EnemyName;
        float cur = bossEnemy.Health.CurrentHealth;
        float max = bossEnemy.Health.MaxHealth;

        var healthUI = _healthUIPool.Get();
        healthUI.Init(bossName, cur, max);

        //딕셔너리에 추가
        _healthUIDict.Add(bossEnemy, healthUI);
    }

    /// <summary>
    /// 보스 체력 UI 제거
    /// </summary>
    public void RemoveBossHealthUI(Enemy bossEnemy)
    {
        if (_healthUIDict.TryGetValue(bossEnemy, out var healthUI))
        {
            _healthUIDict.Remove(bossEnemy);
            _healthUIPool.Release(healthUI);
        }
    }

    /// <summary>
    /// 보스 체력 UI 업데이트
    /// </summary>
    public void UpdateBossHealthUI(Enemy bossEnemy, float currentHealth, float maxHealth)
    {
        if (_healthUIDict.TryGetValue(bossEnemy, out var healthUI))
        {
            healthUI.SetHealth(currentHealth, maxHealth);
        }
    }
    #endregion
}