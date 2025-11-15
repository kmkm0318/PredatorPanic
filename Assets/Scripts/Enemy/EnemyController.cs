using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// NavMeshAgent 컴포넌트를 활용
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyControllerData _enemyControllerData;
    private Transform _target;
    private NavMeshAgent _navMeshAgent;

    public void Init(Enemy enemy, EnemyControllerData enemyControllerData)
    {
        _enemy = enemy;
        _enemyControllerData = enemyControllerData;

        InitComponents();
    }

    private void InitComponents()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _enemy.EnemyStats.GetStat(EnemyStatType.MoveSpeed).FinalValue;
    }

    private void OnEnable()
    {
        if (_enemyControllerData != null)
            StartCoroutine(HandleMovementCoroutine());
    }

    //적 이동 처리. Update에서 호출하게 되면 성능에 영향을 줄 수 있으므로 코루틴으로 처리
    private IEnumerator HandleMovementCoroutine()
    {
        while (true)
        {
            if (_target != null)
            {
                _navMeshAgent.SetDestination(_target.position);
            }
            yield return new WaitForSeconds(_enemyControllerData.PathUpdateRate);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}