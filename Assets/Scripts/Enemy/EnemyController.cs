using UnityEngine;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region 상수
    private const float MIN_ROTATE_MAGNITUDE_SQR = 0.0001f;
    private const float ROTATE_SPEED_RATIO = 0.25f;
    #endregion

    #region 레퍼런스
    private Enemy _enemy;
    private Transform _target;
    #endregion

    #region 데이터
    private EnemyControllerData _enemyControllerData;
    #endregion

    #region 이동 변수
    private bool _isMoving = false;
    private Vector3 _direction = Vector3.zero;
    private float _speed = 0f;
    private float _rotateSpeed = 0f;
    #endregion

    public void Init(Enemy enemy)
    {
        //데이터 할당
        _enemy = enemy;
        _enemyControllerData = enemy.EnemyData.EnemyControllerData;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        //이동 중이 아니면 리턴, 타겟이 없으면 리턴
        if (!_isMoving || _target == null) return;

        //타겟까지의 벡터 계산
        var toTarget = _target.position - transform.position;

        //타겟과의 거리 제곱 계산
        float distanceSqr = toTarget.sqrMagnitude;

        //최소 이동 거리 이내면 리턴
        if (distanceSqr < _enemyControllerData.MinMoveDistanceSqr) return;

        //속도 가져오기
        _speed = _enemy.EnemyStats.GetStat(EnemyStatType.MoveSpeed).FinalValue;

        //회전 속도 계산
        _rotateSpeed = _speed * ROTATE_SPEED_RATIO;

        HandleDirection(toTarget);

        //이동 적용
        transform.position += _direction * _speed * Time.deltaTime;
    }

    #region 방향 처리
    private void HandleDirection(Vector3 toTarget)
    {
        if (_enemyControllerData.MoveType == EnemyMoveType.Walking)
        {
            //지상 이동일 경우 수평 방향으로만 회전
            toTarget.y = 0;
        }

        //정규화
        toTarget.Normalize();

        //이미 거의 같은 방향이면 리턴
        if ((_direction - toTarget).sqrMagnitude < MIN_ROTATE_MAGNITUDE_SQR) return;

        //현재 방향과 목표 방향 사이의 회전 계산
        _direction = Vector3.Lerp(_direction, toTarget, _rotateSpeed * Time.deltaTime);

        //정규화
        _direction.Normalize();

        //방향이 없으면 리턴
        if (_direction == Vector3.zero) return;

        //회전
        transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);
    }
    #endregion

    #region 타겟 설정
    public void SetTarget(Transform target)
    {
        //타겟이 null이면 리턴
        if (target == null) return;

        //타겟 설정
        _target = target;

        //_target 방향 즉시 바라보기
        LookTarget();

        //이동 시작
        _isMoving = true;
    }

    private void LookTarget()
    {
        //목표 방향 구하기
        var targetdir = _target.position - transform.position;

        if (_enemyControllerData.MoveType == EnemyMoveType.Walking)
        {
            //지상 이동일 경우 수평 방향으로만 회전
            targetdir.y = 0;
        }

        //정규화
        targetdir.Normalize();

        //이미 거의 같은 방향이면 리턴
        if ((_direction - targetdir).sqrMagnitude < MIN_ROTATE_MAGNITUDE_SQR) return;

        //현재 방향을 목표 방향으로 설정
        _direction = targetdir;

        //방향이 Vector3.zero면 리턴
        if (_direction == Vector3.zero) return;

        //회전
        transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);
    }
    #endregion
}