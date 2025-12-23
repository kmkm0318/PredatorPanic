using System;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 게임 씬에서 사용하는 매니저 클래스
/// 플레이어와 카메라를 초기화
/// 플레이어에게 기본 무기 장착
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private GameData _gameData;
    public GameData GameData => _gameData;

    [Header("Player")]
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private WeaponData _weaponData;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [Header("Managers")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private DropItemManager _dropItemManager;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private BulletManager _bulletManager;
    [SerializeField] private TrailManager _trailManager;
    [SerializeField] private ExplosionManager _explosionManager;
    [SerializeField] private GameUIManager _gameUIManager;
    public EnemyManager EnemyManager => _enemyManager;
    public DropItemManager DropItemManager => _dropItemManager;
    public ShopManager ShopManager => _shopManager;
    public BulletManager BulletManager => _bulletManager;
    public TrailManager TrailManager => _trailManager;
    public ExplosionManager ExplosionManager => _explosionManager;
    public GameUIManager GameUIManager => _gameUIManager;

    #region 플레이어
    public Player Player { get; private set; }
    #endregion

    #region 상태기계
    public StateMachine StateMachine { get; private set; }
    private GameStateFactory stateFactory;
    #endregion

    #region 게임 데이터
    private int _currentRound = 0;
    public int CurrentRound
    {
        get => _currentRound; set
        {
            _currentRound = value;
            OnCurrentRoundChanged?.Invoke(_currentRound);
        }
    }
    private float _roundTimer = 0f;
    public float RoundTimer
    {
        get => _roundTimer;
        set
        {
            _roundTimer = value;
            OnRoundTimerChanged?.Invoke(_roundTimer);
        }
    }
    #endregion

    #region 이벤트
    public event Action<int> OnCurrentRoundChanged;
    public event Action<float> OnRoundTimerChanged;
    #endregion

    private void Start()
    {
        InitStateMachine();
    }

    #region 초기화
    // 상태기계 초기화
    private void InitStateMachine()
    {
        StateMachine = new StateMachine();
        stateFactory = new GameStateFactory(this);
        StateMachine.ChangeState(stateFactory.Loading);
    }

    //초기화
    public void Init()
    {
        InitPlayer();
        InitCamera();
        InitManagers();

        CurrentRound = 0;
    }

    // 플레이어 생성 및 초기화
    private void InitPlayer()
    {
        //초기 위치는 (0,0,0)
        var spawnPos = Vector3.zero;

        //플레이어 생성
        Player = Instantiate(_playerData.PlayerPrefab, spawnPos, Quaternion.identity);

        //플레이어 초기화
        Player.Init(_playerData, this);

        //기본 무기 장착
        Player.TryAddWeapon(_weaponData);
    }

    // 시네머신 카메라 초기화
    private void InitCamera()
    {
        if (_cinemachineCamera != null)
        {
            Player.InitCamera(_cinemachineCamera);
        }
    }

    //매니저 클래스 초기화. UI는 마지막에 초기화
    private void InitManagers()
    {
        _enemyManager.Init(this);
        _dropItemManager.Init(this);
        _shopManager.Init(this);
        _bulletManager.Init(this);
        _trailManager.Init(this);
        _explosionManager.Init(this);
        _gameUIManager.Init(this);
    }
    #endregion

    private void Update()
    {
        StateMachine.Update();
    }
}