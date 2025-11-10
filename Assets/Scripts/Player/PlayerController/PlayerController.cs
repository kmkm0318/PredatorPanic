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
    private PlayerVisual _playerVisual;
    private Transform _cameraPivot;
    private Transform _weaponPivot;
    #endregion

    #region 컨트롤 변수
    public float Gravity { get; private set; } = -9.8f;
    public float Pitch { get; private set; } = 0f;
    public float InitialJumpSpeed { get; private set; }
    private Vector3 _movement;
    public float MovementX { get => _movement.x; set => _movement.x = value; }
    public float MovementY { get => _movement.y; set => _movement.y = value; }
    public float MovementZ { get => _movement.z; set => _movement.z = value; }
    #endregion

    #region 컴포넌트
    public PlayerInputActions InputActions { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator Animator { get; private set; }
    #endregion

    #region 애니메이션 이름, 해시
    private const string IS_MOVING = "isMoving";
    private const string IS_JUMPING = "isJumping";
    private const string IS_FALLING = "isFalling";
    public int IsMovingHash { get; private set; }
    public int IsJumpingHash { get; private set; }
    public int IsFallingHash { get; private set; }
    #endregion

    #region 상태 기계
    public StateMachine StateMachine { get; private set; }
    public PlayerStateFactory StateFactory { get; private set; }
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
    #endregion

    private void OnEnable()
    {
        RegisterInputCallbackFunction();
    }

    private void OnDisable()
    {
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
        _player = player;
        PlayerControllerData = playerControllerData;
        _playerVisual = playerVisual;
        _cameraPivot = _playerVisual.CameraPivot;
        _weaponPivot = _playerVisual.WeaponPivot;

        InitComponents();
        InitJumpVariables();
        InitAnimationHash();
        InitStateMachine();
    }

    //컴포넌트 초기화
    private void InitComponents()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = _playerVisual.GetComponent<Animator>();
    }

    //점프 변수 초기화. 중력과 시작 점프 속도 결정
    private void InitJumpVariables()
    {
        float timeToApex = PlayerControllerData.MaxJumpDuration / 2f;
        Gravity = -2 * PlayerControllerData.MaxJumpHeight / Mathf.Pow(timeToApex, 2);
        InitialJumpSpeed = 2 * PlayerControllerData.MaxJumpHeight / timeToApex;
    }

    //애니메이션 해시 초기화
    private void InitAnimationHash()
    {
        IsMovingHash = Animator.StringToHash(IS_MOVING);
        IsJumpingHash = Animator.StringToHash(IS_JUMPING);
        IsFallingHash = Animator.StringToHash(IS_FALLING);
    }

    private void InitStateMachine()
    {
        StateMachine = new StateMachine();
        StateFactory = new PlayerStateFactory(this);
        StateMachine.ChangeState(StateFactory.Fall());
    }

    private void Update()
    {
        //바라보는 방향에 따라 움직이는 방향이 결정되기 때문에 HandleRotation 뒤에 HandleMovement가 와야 함
        HandleRotation();
        HandleMovement();

        StateMachine.Update();
    }

    private void HandleRotation()
    {
        float sensitivity = _currentDecive is Mouse ? PlayerControllerData.MouseSensitivity : PlayerControllerData.ControllerRotateSpeed;

        //좌우 회전은 플레이어를 직접 회전
        float yaw = LookInput.x * sensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up, yaw);

        //상하 회전은 카메라 피벗을 회전. 카메라 피벗이 있을 경우에만 실행
        Pitch -= LookInput.y * sensitivity * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch, PlayerControllerData.PitchMin, PlayerControllerData.PitchMax);

        if (_cameraPivot != null)
        {
            _cameraPivot.localEulerAngles = new(Pitch, 0, 0);
        }

        //카메라 피벗에 맞춰 무기 피벗도 회전
        if (_weaponPivot != null)
        {
            _weaponPivot.localEulerAngles = new(Pitch, 0, 0);
        }
    }

    private void HandleMovement()
    {
        //플레이어가 바라보는 방향을 기준으로 이동
        Vector3 localMove = transform.right * _movement.x + Vector3.up * _movement.y + transform.forward * _movement.z;
        CharacterController.Move(PlayerControllerData.MoveSpeed * Time.deltaTime * localMove);
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
}