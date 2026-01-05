using System.Collections;
using UnityEngine;

/// <summary>
/// 폭발 클래스
/// 무기에 따라 불릿에 의해 폭발 효과를 냄
/// </summary>
public class Explosion : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Transform ExplosionVisual;

    #region 데이터
    public ExplosionData ExplosionData { get; private set; }
    #endregion

    #region 레퍼런스 변수
    private ExplosionManager _explosionManager;
    #endregion

    #region 라이프타임
    private bool _isLifetimeRunning = false;
    private float _lifetimeElapsed = 0f;
    private float _lifetimeDuration = 0f;
    #endregion

    public void Init(ExplosionData explosionData, ExplosionManager explosionManager)
    {
        ExplosionData = explosionData;
        _explosionManager = explosionManager;
    }

    private void Update()
    {
        HandleLifetime();
    }

    #region 폭발
    public void Explode(in ExplosionExplodeContext context)
    {
        //위치 설정
        transform.position = context.ExplodePosition;

        //폭발 비주얼 크기 설정
        ExplosionVisual.localScale = 2f * context.Radius * Vector3.one;

        //폭발 효과음 재생
        AudioManager.Instance.PlaySfx(ExplosionData.ExplodeSfxData, transform.position);

        //피해 적용
        int hitCount = PhysicsUtility.GetOverlapSphereNonAlloc(transform.position, context.Radius, context.HitLayerMask, out var colliders);

        for (int i = 0; i < hitCount; i++)
        {
            //충돌한 콜라이더 가져오기
            var collider = colliders[i];

            //적 캐릭터인지 확인
            if (!collider.TryGetComponent<Enemy>(out var enemy)) continue;

            //데미지 가져오기
            float damage = context.BaseDamage;

            //치명타 여부 결정
            bool isCritical = context.CriticalRate.ChanceTest();

            //치명타 데미지 적용
            if (isCritical)
            {
                damage *= context.CriticalDamageRate;
            }

            //데미지 출처 타입 결정
            //무기가 null일 경우 이펙트 데미지로 간주
            var damageSourceType = context.Weapon == null ? PlayerDamageSourceType.Effect : PlayerDamageSourceType.Explosion;

            //데미지 컨텍스트 생성
            PlayerDamageContext damageContext = new(
                context.Player,
                context.Weapon,
                enemy,
                enemy.CenterPosition,
                damage,
                isCritical,
                damageSourceType
            );

            //데미지 적용
            enemy.TakeDamage(damageContext);
        }

        //폭발 라이프타임 시작
        StartLifetime(ExplosionData.VisualDuration);
    }
    #endregion

    #region 라이프타임
    private void HandleLifetime()
    {
        //비활성화 시 패스
        if (!gameObject.activeSelf) return;

        //라이프타임이 동작 중이지 않으면 패스
        if (!_isLifetimeRunning) return;

        //경과 시간 누적
        _lifetimeElapsed += Time.deltaTime;

        //지정된 지속 시간 도달 시 폭발 반환
        if (_lifetimeElapsed >= _lifetimeDuration)
        {
            StopLifetime();
        }
    }

    private void StartLifetime(float duration)
    {
        //기존 라이프타임 중지
        StopLifetime();

        //변수 설정
        _lifetimeDuration = duration;
        _lifetimeElapsed = 0f;
        _isLifetimeRunning = true;
    }

    private void StopLifetime()
    {
        //라이프 타임이 동작 중이지 않으면 패스
        if (!_isLifetimeRunning) return;

        //변수 초기화
        _isLifetimeRunning = false;

        //폭발 반환
        _explosionManager.ReleaseExplosion(this);
    }
    #endregion
}