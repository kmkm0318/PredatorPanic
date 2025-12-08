using System.Collections;
using System.Collections.Generic;
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
    private Transform _cameraPivot;
    private List<Transform> _weaponPivots;
    #endregion

    #region 컨트롤 변수
    public float Gravity { get; private set; } = -9.8f;
    public float InitialJumpSpeed { get; private set; }
    public int AirJumpRemain { get; private set; }
    private float _pitch = 0f;
    public bool IsGrounded => _characterController.isGrounded;
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
    private bool _canAttack = true;
    public bool CanAttack
    {
        get => _canAttack;
        set
        {
            _canAttack = value;
            if (!value)
            {
                _player.StopAttack();
            }
        }
    }
    private InputDevice _currentDecive;
    #endregion

    #region 코루틴
    private Coroutine _jumpBufferCoroutine;
    private Coroutine _coyoteTimeCoroutine;
    private Coroutine _groundCheckCoroutine;
    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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

        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.Attack.canceled += OnAttack;
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

        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Attack.canceled -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        IsMovePressed = MoveInput.x != 0 || MoveInput.y != 0;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _currentDecive = context.control.device;

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

    private void OnAttack(InputAction.CallbackContext context)
    {
        IsAttackPressed = context.ReadValueAsButton();

        if (CanAttack)
        {
            if (IsAttackPressed)
            {
                _player.StartAttack();
            }
            else
            {
                _player.StopAttack();
            }
        }
    }
    #endregion

    public void Init(Player player, PlayerControllerData playerControllerData, PlayerVisual playerVisual)
    {
        //참조 데이터 할당
        _player = player;
        PlayerControllerData = playerControllerData;
        PlayerVisual = playerVisual;

        //Player Visual에서 카메라 피벗과 무기 피벗 참조
        _cameraPivot = PlayerVisual.CameraPivot;
        _weaponPivots = PlayerVisual.WeaponPivots;

        //점프 관련 변수 초기화
        InitJumpVariables();

        //상태 기계 초기화
        InitStateMachine();
    }

    #region 점프 관련 함수
    //점프 변수 초기화. 중력과 시작 점프 속도 결정. 공중 점프 횟수 초기화
    private void InitJumpVariables()
    {
        float timeToApex = PlayerControllerData.MaxJumpDuration / 2f;
        Gravity = -2 * PlayerControllerData.MaxJumpHeight / Mathf.Pow(timeToApex, 2);
        InitialJumpSpeed = 2 * PlayerControllerData.MaxJumpHeight / timeToApex;

        ResetJumpRemain();
    }

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

    //상태 기계 초기화. 낙하 상태로 시작
    private void InitStateMachine()
    {
        StateMachine = new StateMachine();
        stateFactory = new PlayerStateFactory(this);
        StateMachine.ChangeState(stateFactory.Fall);
    }

    private void Update()
    {
        //플레이어 회전 처리
        HandleRotation();

        //플레이어 이동 처리
        HandleMovement();

        StateMachine.Update();
    }

    private void HandleRotation()
    {
        //입력 장치에 따라 감도 설정
        float sensitivity = _currentDecive is Mouse ? PlayerControllerData.MouseSensitivityMultiplier : PlayerControllerData.ControllerRotateSpeedMultiplier;

        //좌우 회전은 플레이어를 직접 회전
        float yaw = LookInput.x * sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up, yaw);

        //상하 회전은 카메라 피벗을 회전. 카메라 피벗이 있을 경우에만 실행
        _pitch -= LookInput.y * sensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, PlayerControllerData.PitchMin, PlayerControllerData.PitchMax);

        if (_cameraPivot != null)
        {
            _cameraPivot.localEulerAngles = new(_pitch, 0, 0);
        }

        //카메라 피벗에 맞춰 무기 피벗도 회전
        foreach (var weaponPivot in _weaponPivots)
        {
            weaponPivot.localEulerAngles = new(_pitch, 0, 0);
        }
    }

    private void HandleMovement()
    {
        //시간이 멈춰있으면 이동 처리 안함
        if (Time.timeScale == 0f) return;

        //이동 속도 적용
        float moveSpeed = _player.PlayerStats.GetStat(PlayerStatType.MoveSpeed).FinalValue;

        //로컬 좌표계 기준으로 이동 벡터 계산
        Vector3 horizontalMove = transform.right * _movement.x + transform.forward * _movement.z;
        Vector3 verticalMove = transform.up * _movement.y;
        Vector3 moveVelocity = moveSpeed * horizontalMove + verticalMove;

        //캐릭터 컨트롤러로 이동
        _characterController.Move(moveVelocity * Time.deltaTime);

        $"TimeScale: {Time.timeScale}, Movement: {_movement}, IsGrounded: {IsGrounded}".Log();
    }

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

    private void OnDrawGizmosSelected()
    {
        //그라운드 체크 레이캐스트 시각화
        if (PlayerControllerData != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * PlayerControllerData.GroundCheckDistance);
        }
    }
}