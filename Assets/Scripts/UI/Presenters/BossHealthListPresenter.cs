using System;

/// <summary>
/// 보스 체력 리스트 프리젠터 클래스
/// </summary>
public class BossHealthListPresenter : IPresenter
{
    #region 레퍼런스
    private EnemyManager _enemyManager;
    private BossHealthListUI _bossHealthListUI;
    #endregion

    /// <summary>
    /// 생성자
    /// UIManager에서 주입
    /// </summary>
    public BossHealthListPresenter(EnemyManager enemyManager, BossHealthListUI bossHealthListUI)
    {
        _enemyManager = enemyManager;
        _bossHealthListUI = bossHealthListUI;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        RegisterEvents();

        _bossHealthListUI.Init();
    }

    public void Reset()
    {
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        if (_enemyManager)
        {
            _enemyManager.OnBossSpawned += HandleBossSpawned;
            _enemyManager.OnBossDeath += HandleBossDeath;
            _enemyManager.OnBossHealthChanged += HandleBossHealthChanged;
        }
    }

    private void UnregisterEvents()
    {
        if (_enemyManager)
        {
            _enemyManager.OnBossSpawned -= HandleBossSpawned;
            _enemyManager.OnBossDeath -= HandleBossDeath;
            _enemyManager.OnBossHealthChanged -= HandleBossHealthChanged;
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleBossSpawned(Enemy enemy)
    {
        _bossHealthListUI.AddBossHealthUI(enemy);
    }

    private void HandleBossDeath(Enemy enemy)
    {
        _bossHealthListUI.RemoveBossHealthUI(enemy);
    }

    private void HandleBossHealthChanged(Enemy enemy, float cur, float max)
    {
        _bossHealthListUI.UpdateBossHealthUI(enemy, cur, max);
    }
    #endregion
}