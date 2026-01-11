using UnityEngine;

/// <summary>
/// 적들의 공격을 위한 오브젝트 클래스
/// 지정된 위치에 일정 시간 후 공격을 실행
/// 일정 시간동안 시각적 표시
/// </summary>
public class IndicatedAttack : MonoBehaviour
{
    [Header("Object Referneces")]
    [SerializeField] private Transform _OuterVisual;
    [SerializeField] private Transform _InnerVisual;

    #region 데이터
    public IndicatedAttackData IndicatedAttackData { get; private set; }
    #endregion

    #region 레퍼런스
    private IndicatedAttackManager _indicatedAttackManager;
    #endregion

    #region 공격 변수
    private bool _isAttacking = false;
    private float _radius = 0f;
    private float _delay = 0f;
    private float _timer = 0f;
    private float _damage = 0f;
    #endregion

    public void Init(IndicatedAttackData indicatedAttackData, IndicatedAttackManager manager)
    {
        //데이터 할당
        IndicatedAttackData = indicatedAttackData;
        _indicatedAttackManager = manager;
    }

    private void Update()
    {
        HandleAttack();
    }

    #region 공격
    private void HandleAttack()
    {
        //공격 중이 아니면 패스
        if (!_isAttacking) return;

        //타이머 증가
        _timer += Time.deltaTime;

        //시각적 표시 업데이트
        float progress = _timer / _delay;
        _InnerVisual.localScale = _radius * 2f * progress * Vector3.one;

        //딜레이가 지나면 공격 실행
        if (_timer >= _delay)
        {
            //충돌 감지
            var hitCounts = PhysicsUtility.GetOverlapSphereNonAlloc(transform.position, _radius, IndicatedAttackData.TargetLayer, out var colliders);

            //피해 적용
            for (int i = 0; i < hitCounts; i++)
            {
                var collider = colliders[i];
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }

            //공격 종료
            StopAttack();
        }
    }

    public void StartAttack(Vector3 position, float radius, float delay, float damage)
    {
        //위치 설정
        transform.position = position;

        //딜레이 및 타이머 설정
        _radius = radius;
        _delay = delay;
        _timer = 0f;
        _damage = damage;

        //시각적 표시 초기화
        _OuterVisual.localScale = radius * 2f * Vector3.one;
        _InnerVisual.localScale = Vector3.zero;

        //공격 상태 설정
        _isAttacking = true;
    }

    public void StopAttack()
    {
        //공격 상태 해제
        _isAttacking = false;

        //오브젝트 반환
        _indicatedAttackManager.ReleaseIndicatedAttack(this);
    }
    #endregion
}