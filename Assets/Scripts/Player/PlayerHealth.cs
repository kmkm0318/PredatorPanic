using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어 체력 관리 클래스
/// 무적 시간 등의 특화 기능을 포함합니다.
/// </summary>
public class PlayerHealth : Health, IDamageable
{
    #region 참조 변수
    private Player _player;
    #endregion

    #region 무적
    private bool _isInvincible = true;
    private float _invincibleDuration = 0f;
    #endregion

    #region 코루틴
    private Coroutine _invincibleCoroutine;
    #endregion

    #region 이벤트
    public event Action<bool> OnInvincibleStateChanged;
    public event Action<float> OnTakeDamage;
    #endregion

    #region 초기화
    //플레이어에서 호출
    public void Init(Player player)
    {
        _player = player;

        //플레이어 스탯에서 최대 체력 가져오기
        var maxHealth = player.PlayerStats.GetStat(PlayerStatType.Health).FinalValue;

        //초기화
        Init(maxHealth);

        //무적 시간 가져오기
        _invincibleDuration = player.PlayerStats.GetStat(PlayerStatType.InvincibleDuration).FinalValue;

        //이벤트 지정
        RegisterHealthStatEvents();
    }

    // 체력, 무적 시간 스탯 변경시 Health 컴포넌트에 반영
    private void RegisterHealthStatEvents()
    {
        _player.PlayerStats.GetStat(PlayerStatType.Health).OnValueChanged += (newValue) =>
        {
            SetMaxHealth(newValue);
        };

        _player.PlayerStats.GetStat(PlayerStatType.InvincibleDuration).OnValueChanged += (newValue) =>
        {
            _invincibleDuration = newValue;
        };
    }
    #endregion

    #region 무적 코루틴
    private IEnumerator InvincibleCoroutine()
    {
        yield return new WaitForSeconds(_invincibleDuration);
        StopInvincible();
    }

    //무적 시간 시작
    private void StartInvincible()
    {
        //기존 무적 시간 종료
        StopInvincible();

        //무적 상태 시작
        _isInvincible = true;

        //이벤트 호출
        OnInvincibleStateChanged?.Invoke(_isInvincible);

        //타이머 코루틴 시작
        _invincibleCoroutine = StartCoroutine(InvincibleCoroutine());
    }

    //무적 시간 종료
    private void StopInvincible()
    {
        //코루틴이 실행 중일 때만 종료
        if (_invincibleCoroutine != null)
        {
            //코루틴 종료
            StopCoroutine(_invincibleCoroutine);

            //코루틴 변수 초기화
            _invincibleCoroutine = null;

            //무적 상태 종료
            _isInvincible = false;

            //이벤트 호출
            OnInvincibleStateChanged?.Invoke(_isInvincible);
        }
    }
    #endregion

    public void SetInvincible(bool value)
    {
        //무적 코루틴 중지
        StopInvincible();

        //무적 상태 설정
        _isInvincible = value;
    }

    #region 충돌 처리
    //충돌 처리. 적은 Trigger를 갖고 있음
    private void OnTriggerEnter(Collider other)
    {
        HandleEnemyCollision(other);
    }

    //무적 시간이 끝났을 때 다시 충돌 처리하기 위함
    private void OnTriggerStay(Collider other)
    {
        HandleEnemyCollision(other);
    }

    //적과의 충돌 처리
    private void HandleEnemyCollision(Collider other)
    {
        //무적인 경우, 죽은 경우 무시
        if (_isInvincible || IsDead) return;

        //적일 때
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            //데미지 가져오기
            float damage = enemy.EnemyStats.GetStat(EnemyStatType.Damage).FinalValue;

            //데미지 입기
            TakeDamage(damage);
        }
    }

    public override void TakeDamage(float damage)
    {
        //방어력 가져오기
        float defense = _player.PlayerStats.GetStat(PlayerStatType.Defense).FinalValue;

        //방어력 적용 후 데미지 계산
        damage = CombatUtility.CalculateDefensedDamage(damage, defense);

        //적과 충돌 시 데미지 입기
        base.TakeDamage(damage);

        //데미지 입음 이벤트 호출
        OnTakeDamage?.Invoke(damage);

        if (!IsDead)
        {
            //피격 후 살아있을 때 무적 상태 시작
            StartInvincible();
        }
    }
    #endregion
}