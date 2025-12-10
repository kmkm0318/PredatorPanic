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
    public Health Health { get; private set; }
    #endregion

    #region 적 스탯
    public Stats<EnemyStatType> EnemyStats { get; private set; }
    #endregion

    #region 이벤트
    public event Action<Enemy> OnDeath;
    public event Action<Enemy, float, float> OnHealthChanged;
    #endregion

    private void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyVisual = GetComponentInChildren<EnemyVisual>();
        Health = GetComponent<Health>();

        Health.OnHealthChanged += HandleHealthChanged;
        Health.OnDeath += Die;
    }

    private void HandleHealthChanged(float cur, float max)
    {
        //이벤트 호출
        OnHealthChanged?.Invoke(this, cur, max);
    }

    public void Init(EnemyData enemyData)
    {
        //적 데이터 설정
        EnemyData = enemyData;
    }

    public void SetLevel(int level)
    {
        //레벨에 따른 스탯 재초기화
        //EnemyController에서 Stat를 사용하기 때문에 Stat 먼저 초기화
        InitStats(level);
        InitComponents();
    }

    private void InitStats(int level = 0)
    {
        //기본 스탯 초기화
        EnemyStats = new Stats<EnemyStatType>(EnemyData.BaseStats);

        //레벨에 따른 스탯 증가 적용
        foreach (var entity in EnemyData.IncreaseRates)
        {
            float increaseAmount = entity.Value * level;
            EnemyStats.GetStat(entity.StatType).AddModifier(new(increaseAmount, StatModifierType.PercentAdd, this));
        }
    }

    private void InitComponents()
    {
        //적 컨트롤러 초기화
        _enemyController.Init(this);

        //Health 컴포넌트 초기화
        float maxHealth = EnemyStats.GetStat(EnemyStatType.Health).FinalValue;
        float defense = EnemyStats.GetStat(EnemyStatType.Defense).FinalValue;

        Health.Init(maxHealth, defense);

        //Health 스탯 변경 이벤트 등록
        RegisterHealthStatEvents();
    }

    // 체력, 방어력 스탯 변경시 Health 컴포넌트에 반영
    private void RegisterHealthStatEvents()
    {
        EnemyStats.GetStat(EnemyStatType.Health).OnValueChanged += (newValue) =>
        {
            Health.SetMaxHealth(newValue);
        };

        EnemyStats.GetStat(EnemyStatType.Defense).OnValueChanged += (newValue) =>
        {
            Health.SetDefense(newValue);
        };
    }

    /// <summary>
    /// 플레이어가 적에게 피해를 주는 경우
    /// </summary>
    public void TakeDamage(PlayerDamageContext context)
    {
        //방어력 적용 및 현재 체력을 최대값으로 제한
        context.Damage = Health.TakeDamage(context.Damage);

        //플레이어에게 적중 처리 알림
        context.Player.HandleOnHit(context);

        if (Health.IsDead)
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
        //사망 이벤트 호출
        OnDeath?.Invoke(this);
    }

    public void SetTarget(Transform target)
    {
        _enemyController.SetTarget(target);
    }
}