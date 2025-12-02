using System;
using UnityEngine;

/// <summary>
/// 적 클래스
/// 적 데이터로 초기화되고 적의 컨트롤러와 체력 컴포넌트를 관리합니다.
/// 레벨에 따라 스탯이 증가합니다.
/// </summary>
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    #region 데이터
    public EnemyData EnemyData { get; private set; }
    #endregion

    #region 컴포넌트
    private EnemyController _enemyController;
    private EnemyVisual _enemyVisual;
    private Health _health;
    #endregion

    #region 적 스탯
    public Stats<EnemyStatType> EnemyStats { get; private set; }
    #endregion

    #region 이벤트
    public static event Action<Enemy> OnAnyDeath;
    public static event Action<Enemy> OnAnyReleaseRequested;
    #endregion

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyVisual = GetComponentInChildren<EnemyVisual>();

        _health = GetComponent<Health>();
        _health.OnDeath += Die;
    }

    public void Init(EnemyData enemyData, int level = 0)
    {
        EnemyData = enemyData;

        //EnemyController에서 Stat를 사용하기 때문에 Stat 먼저 초기화
        InitStats(level);
        InitComponents();
    }

    private void InitStats(int level = 0)
    {
        EnemyStats = new Stats<EnemyStatType>(EnemyData.BaseStats);

        foreach (var entity in EnemyData.IncreaseRates)
        {
            float increaseAmount = entity.Value * level;
            EnemyStats.GetStat(entity.StatType).AddModifier(new(increaseAmount, StatModifierType.PercentAdd, this));
        }
    }

    private void InitComponents()
    {
        _enemyController.Init(this, EnemyData.EnemyControllerData);

        float maxHealth = EnemyStats.GetStat(EnemyStatType.Health).FinalValue;
        float defense = EnemyStats.GetStat(EnemyStatType.Defense).FinalValue;

        _health.Init(maxHealth, defense);

        RegisterHealthStatEvents();
    }

    // 체력, 방어력 스탯 변경시 Health 컴포넌트에 반영
    private void RegisterHealthStatEvents()
    {
        EnemyStats.GetStat(EnemyStatType.Health).OnValueChanged += (newValue) =>
        {
            _health.SetMaxHealth(newValue);
        };

        EnemyStats.GetStat(EnemyStatType.Defense).OnValueChanged += (newValue) =>
        {
            _health.SetDefense(newValue);
        };
    }

    /// <summary>
    /// 플레이어가 적에게 피해를 주는 경우
    /// </summary>
    public void TakeDamage(PlayerDamageContext context)
    {
        //데미지 적용
        context.Enemy = this;

        //방어력 적용 및 현재 체력을 최대값으로 제한
        context.Damage = _health.TakeDamage(context.Damage);

        //플레이어에게 적중 처리 알림
        context.Player.HandleOnHit(context);

        if (_health.IsDead)
        {
            //사망 시 플레이어에게 처치 처리 알림
            context.Player.HandleOnKill(context);
        }
        else
        {
            //피격 플래시 재생
            _enemyVisual.StartHitFlash();
        }
    }

    // 적 사망 처리
    // 체력이 0이 되거나 EnemyManager에서 호출
    public void Die()
    {
        OnAnyDeath?.Invoke(this);
        OnAnyReleaseRequested?.Invoke(this);
    }

    public void SetTarget(Transform target)
    {
        _enemyController.SetTarget(target);
    }
}