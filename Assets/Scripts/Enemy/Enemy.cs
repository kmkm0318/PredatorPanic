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
    #region 적의 중심
    //적 센터, 총알의 발사 방향을 계산하는 데 사용
    [SerializeField] private Transform _centerTransform;
    public Vector3 CenterPosition => _centerTransform.position;
    #endregion
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

        //성장률 가져오기
        float growthRate = EnemyData.EnemyStatGrowthRate;

        //레벨에 따른 성장 값 계산
        float growthValue = Mathf.Pow(growthRate, level);

        //레벨에 따른 스탯 증가 적용
        foreach (var type in Enum.GetValues(typeof(EnemyStatType)))
        {
            //스탯 가져오기
            var stat = EnemyStats.GetStat((EnemyStatType)type);

            //모디파이어 추가
            stat.AddModifier(new(growthValue, StatModifierType.PercentMult, this));
        }
    }

    private void InitComponents()
    {
        //적 컨트롤러 초기화
        _enemyController.Init(this);

        //Health 컴포넌트 초기화
        float maxHealth = EnemyStats.GetStat(EnemyStatType.Health).FinalValue;

        Health.Init(maxHealth);
    }

    /// <summary>
    /// 플레이어가 적에게 피해를 주는 경우
    /// </summary>
    public void TakeDamage(in PlayerDamageContext context)
    {
        //데미지 적용
        Health.TakeDamage(context.Damage);

        //플레이어에게 적중 처리 알림
        context.Player.HandleOnHit(context);

        if (Health.IsDead)
        {
            //사망 사운드 재생
            AudioManager.Instance.PlaySfx(EnemyData.DeathAudio, CenterPosition);

            //사망 파티클 이펙트 재생
            ParticleEffectManager.Instance.Play(EnemyData.DeathParticleEffect, CenterPosition, transform.rotation);

            //사망 시 플레이어에게 처치 처리 알림
            context.Player.HandleOnKill(context);
        }
        else
        {
            //피격 플래시 재생
            _enemyVisual.StartHitFlash();

            //피격 사운드 재생
            AudioManager.Instance.PlaySfx(EnemyData.HitAudio, CenterPosition);

            //피격 파티클 이펙트 재생
            ParticleEffectManager.Instance.Play(EnemyData.HitParticleEffect, CenterPosition, transform.rotation);
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

    public void OnSpawn()
    {
        //스폰 비주얼 애니메이션 실행
        _enemyVisual.PlaySpawnAnimation(EnemyData.SpawnVisualOffsetY, EnemyData.SpawnVisualDuration, EnemyData.SpawnVisualEase);
    }
}