using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 플레이어 컨트롤러 클래스
/// 플레이어의 입력 처리, 이동, 상태 기계 관리
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region 데이터 및 참조 변수
    private Player _player;
    public PlayerControllerData PlayerControllerData { get; private set; }
    public PlayerVisual PlayerVisual { get; private set; }
    private Transform _cameraTransform;
    #endregion

    #region 컨트롤 변수
    public float InitialJumpSpeed => _player.PlayerStats.GetStat(PlayerStatType.JumpForce).FinalValue;
    public int AirJumpRemain { get; private set; }
    public bool IsGrounded { get; private set; } = false;
    private Vector3 _movement;
    public float MovementX { get => _movement.x; set => _movement.x = value; }
    public float MovementY { get => _movement.y; set => _movement.y = value; }
    public float MovementZ { get => _movement.z; set => _movement.z = value; }
    #endregion

    #region 컴포넌트
    private CharacterController _characterController;
    #endregion

    #region 상태 기계
    public StateMachine StateMachine { get; private set; }
    private PlayerStateFactory stateFactory;
    #endregion

    #region 입력
    public Vector2 MoveInput { get; private set; }
    public bool IsMovePressed { get; private set; } = false;
    public Vector2 LookInput { get; private set; }
    public bool IsJumpPressed { get; private set; } = false;
    public bool IsJumpBuffer { get; set; } = false;
    public bool IsCoyoteTime { get; set; } = false;
    public bool IsAttackPressed { get; private set; } = false;
    #endregion

    #region 코루틴
    private Coroutine _jumpBufferCoroutine;
    private Coroutine _coyoteTimeCoroutine;
    #endregion

    private void Awake()
    {
        //캐릭터 컨트롤러 컴포넌트 할당
        _characterController = GetComponent<CharacterController>();

        //카메라 트랜스폼 할당
        _cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        //입력 이벤트 구독
        RegisterInputCallbackFunction();
    }

    private void OnDisable()
    {
        //입력 이벤트 구독 해제
        UnregisterInputCallbackFunction();
    }

    #region 입력 이벤트
    //입력 이벤트 구독
    private void RegisterInputCallbackFunction()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJump;
    }

    //입력 이벤트 구독 해제
    private void UnregisterInputCallbackFunction()
    {
        var inputActions = InputManager.Instance.PlayerInputActions;

        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;

        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Jump.canceled -= OnJump;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        IsMovePressed = MoveInput.x != 0 || MoveInput.y != 0;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        IsJumpPressed = context.ReadValueAsButton();

        if (IsJumpPressed)
        {
            StartJumpBufferCoroutine();
        }
        else
        {
            StopJumpBufferCoroutine();
        }
    }
    #endregion

    #region 초기화
    public void Init(Player player)
    {
        //참조 데이터 할당
        _player = player;
        PlayerControllerData = _player.PlayerData.PlayerControllerData;
        PlayerVisual = _player.PlayerVisual;

        //상태 기계 초기화
        InitStateMachine();
    }

    //상태 기계 초기화. 낙하 상태로 시작
    private void InitStateMachine()
    {
        StateMachine = new StateMachine();
        stateFactory = new PlayerStateFactory(this);
        StateMachine.ChangeState(stateFactory.Fall);
    }
    #endregion

    #region 점프 관련 함수
    //공중 점프 남은 횟수 초기화.
    public void ResetJumpRemain()
    {
        AirJumpRemain = Mathf.FloorToInt(_player.PlayerStats.GetStat(PlayerStatType.AirJumpCount).FinalValue);
    }

    //공중 점프 시도. Fall 상태에서 사용.
    public bool TryAirJump()
    {
        if (AirJumpRemain > 0)
        {
            AirJumpRemain--;
            return true;
        }
        return false;
    }
    #endregion

    private void Update()
    {
        //플레이어 이동 처리
        HandleMovement();

        StateMachine.Update();
    }

    #region 움직임 처리
    private void HandleMovement()
    {
        //정지 상태인 경우 패스
        if (Time.deltaTime <= 0f) return;

        //카메라 트랜스폼이 없는 경우 패스
        if (_cameraTransform == null) return;

        //이동 속도 가져오기
        float moveSpeed = _player.PlayerStats.GetStat(PlayerStatType.MoveSpeed).FinalValue;

        //카메라를 기준으로 정면, 오른쪽 계산
        var cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        var cameraRight = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;

        //이동 벡터 계산
        Vector3 horizontalMove = cameraRight * _movement.x + cameraForward * _movement.z;
        Vector3 verticalMove = transform.up * _movement.y;
        Vector3 moveVelocity = moveSpeed * horizontalMove + verticalMove;

        //캐릭터 컨트롤러로 이동
        _characterController.Move(moveVelocity * Time.deltaTime);

        //지상 체크 업데이트
        IsGrounded = _characterController.isGrounded;

        //플레이어 회전 처리
        HandleRotation(horizontalMove);
    }

    //플레이어 회전 처리
    private void HandleRotation(Vector3 horizontalMove)
    {
        //이동 벡터가 거의 없는 경우 회전하지 않음
        if (horizontalMove.sqrMagnitude <= 0.01f) return;

        //목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(horizontalMove);

        //부드럽게 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, PlayerControllerData.RotationSpeed * Time.deltaTime);
    }
    #endregion

    #region 점프 버퍼 코루틴
    private IEnumerator JumpBufferCoroutine()
    {
        yield return new WaitForSeconds(PlayerControllerData.JumpBufferTime);
        StopJumpBufferCoroutine();
    }

    public void StartJumpBufferCoroutine()
    {
        StopJumpBufferCoroutine();
        IsJumpBuffer = true;
        _jumpBufferCoroutine = StartCoroutine(JumpBufferCoroutine());
    }

    public void StopJumpBufferCoroutine()
    {
        if (_jumpBufferCoroutine != null)
        {
            StopCoroutine(_jumpBufferCoroutine);
            _jumpBufferCoroutine = null;
            IsJumpBuffer = false;
        }
    }
    #endregion

    #region 코요테 타임 코루틴
    private IEnumerator CoyoteTimeCoroutine()
    {
        yield return new WaitForSeconds(PlayerControllerData.CoyoteTime);
        StopCoyoteTimeCoroutine();
    }

    public void StartCoyoteTimeCoroutine()
    {
        StopCoyoteTimeCoroutine();
        IsCoyoteTime = true;
        _coyoteTimeCoroutine = StartCoroutine(CoyoteTimeCoroutine());
    }

    public void StopCoyoteTimeCoroutine()
    {
        if (_coyoteTimeCoroutine != null)
        {
            StopCoroutine(_coyoteTimeCoroutine);
            _coyoteTimeCoroutine = null;
            IsCoyoteTime = false;
        }
    }
    #endregion
}