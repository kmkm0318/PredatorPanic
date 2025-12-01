using System.Collections;
using UnityEngine;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// </summary>
public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyControllerData _enemyControllerData;
    private Transform _target;
    private Planet _planet;

    public void Init(Enemy enemy, EnemyControllerData enemyControllerData, Planet planet)
    {
        _enemy = enemy;
        _enemyControllerData = enemyControllerData;
        _planet = planet;

        //행성에 타겟 등록
        if (_planet != null)
        {
            _planet.RegisterTarget(transform);
        }
    }

    //적 방향 처리. 타겟을 향해 회전
    private IEnumerator HandleRotationCoroutine()
    {
        while (true)
        {
            //타겟 바라보기
            LookTarget();
            yield return new WaitForSeconds(_enemyControllerData.PathUpdateRate);
        }
    }

    //타겟 바라보기
    private void LookTarget()
    {
        if (_target == null) return;

        var toTarget = _planet.GetToTargetRotation(transform, _target);
        transform.rotation = toTarget;
    }

    //타겟을 지정하는 함수
    public void SetTarget(Transform target)
    {
        _target = target;

        //_target 방향 즉시 바라보기
        LookTarget();

        StartCoroutine(HandleRotationCoroutine());
    }
}