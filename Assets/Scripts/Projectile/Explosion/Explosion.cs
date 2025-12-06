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

    public void Init(ExplosionData explosionData, ExplosionManager explosionManager)
    {
        ExplosionData = explosionData;
        _explosionManager = explosionManager;
    }

    #region 폭발 및 반환
    public void Explode(ExplosionExplodeContext context)
    {
        //폭발 비주얼 크기 설정
        ExplosionVisual.localScale = 2f * context.Radius * Vector3.one;

        int hitCount = CombatUtility.GetOverlapSphereNonAlloc(transform.position, context.Radius, context.HitLayerMask, out var colliders);

        for (int i = 0; i < hitCount; i++)
        {
            var collider = colliders[i];

            if (!collider.TryGetComponent<Enemy>(out var enemy)) continue;

            //거리 비례 데미지 계산
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            float damage = CombatUtility.CalculateRangedDamage(context.BaseDamage, context.Radius, distance);

            //치명타 여부 결정
            bool isCritical = Random.value <= context.CriticalRate;
            if (isCritical)
            {
                damage *= context.CriticalDamageRate;
            }

            //데미지 컨텍스트 생성
            PlayerDamageContext damageContext = new()
            {
                Player = context.Weapon.Player,
                Weapon = context.Weapon,
                Enemy = enemy,
                Damage = damage, //방어력을 적용하기 전 데미지
                IsCritical = isCritical,
            };

            //데미지 적용
            enemy.TakeDamage(damageContext);
        }

        StartCoroutine(DelayedRelease());
    }

    //지연 반환 코루틴
    private IEnumerator DelayedRelease()
    {
        //지연 대기
        yield return new WaitForSeconds(ExplosionData.VisualDuration);

        //폭발 반환
        _explosionManager.ReleaseExplosion(this);
    }
    #endregion
}