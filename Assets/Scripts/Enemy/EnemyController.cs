using UnityEngine;

/// <summary>
/// 적 컨트롤러 클래스
/// 적의 이동 및 타겟 추적 기능을 담당
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region 상수
    private const float MIN_ROTATE_MAGNITUDE_SQR = 0.0001f;
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
    private float _speed = 0f;
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

        //타겟과의 거리 계산
        float distanceSqr = (_target.position - transform.position).sqrMagnitude;

        //최소 이동 거리 이내면 리턴
        if (distanceSqr < _enemyControllerData.MinMoveDistanceSqr) return;

        //속도 가져오기
        _speed = _enemy.EnemyStats.GetStat(EnemyStatType.MoveSpeed).FinalValue;

        HandleRotation();
        HandleHorizontalMovement();
        HandleVerticalMovement();
    }

    #region 회전
    private void HandleRotation()
    {
        //수평으로만 회전
        var dir = _target.position - transform.position;
        dir.y = 0;

        //최소 회전 벡터 크기 이하이면 리턴
        if (dir.sqrMagnitude < MIN_ROTATE_MAGNITUDE_SQR) return;

        //목표 방향 계산
        var toTarget = Quaternion.LookRotation(dir, Vector3.up);

        //부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, toTarget, Time.deltaTime * _speed);
    }
    #endregion

    #region 움직임
    private void HandleHorizontalMovement()
    {
        //앞으로 이동
        var velocity = transform.forward * _speed;

        //이동
        transform.position += velocity * Time.deltaTime;
    }

    private void HandleVerticalMovement()
    {
        //지상 이동일 경우 패스
        if (_enemyControllerData.MoveType == EnemyMoveType.Walking) return;

        //높이 차이 계산
        float heightDiff = _target.position.y - _enemy.transform.position.y;

        //이동 방향 설정
        var dir = heightDiff > 0 ? Vector3.up : Vector3.down;

        //프레임 이동량 계산
        //거리 이상으로 움직이지 않도록 제한
        float moveAmount = Mathf.Min(Mathf.Abs(heightDiff), _speed * Time.deltaTime);

        //이동
        transform.position += dir * moveAmount;
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
        //수평으로만 회전
        var dir = _target.position - transform.position;
        dir.y = 0;

        //최소 회전 벡터 크기 이하이면 리턴
        if (dir.sqrMagnitude < MIN_ROTATE_MAGNITUDE_SQR) return;

        //목표 방향으로 회전
        var toTarget = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = toTarget;
    }
    #endregion
}