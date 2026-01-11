using System;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 게임 씬에서 사용하는 게임 매니저 클래스
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private GameData _gameData;
    public GameData GameData => _gameData;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [Header("Managers")]
    [SerializeField] private GameUIManager _gameUIManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private DropItemManager _dropItemManager;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private BulletManager _bulletManager;
    [SerializeField] private TrailManager _trailManager;
    [SerializeField] private ExplosionManager _explosionManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private IndicatedAttackManager _indicatedAttackManager;
    public GameUIManager GameUIManager => _gameUIManager;
    public EnemyManager EnemyManager => _enemyManager;
    public DropItemManager DropItemManager => _dropItemManager;
    public ShopManager ShopManager => _shopManager;
    public BulletManager BulletManager => _bulletManager;
    public TrailManager TrailManager => _trailManager;
    public ExplosionManager ExplosionManager => _explosionManager;
    public CameraManager CameraManager => _cameraManager;
    public IndicatedAttackManager IndicatedAttackManager => _indicatedAttackManager;

    #region 플레이어
    public Player Player { get; private set; }
    #endregion

    #region 상태기계
    public StateMachine StateMachine { get; private set; }
    private GameStateFactory stateFactory;
    #endregion

    #region 게임 데이터
    public int TargetRound { get; private set; }
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
        InitRoundData();
    }

    // 플레이어 생성 및 초기화
    private void InitPlayer()
    {
        //초기 위치는 (0,0,0)
        var spawnPos = Vector3.zero;

        //선택된 플레이어
        var playerData = GlobalGameManager.Instance.SelectedPlayerData;

        //선택된 무기
        var weaponData = GlobalGameManager.Instance.SelectedWeaponData;

        //적용된 진화들
        var appliedEvolutions = GlobalGameManager.Instance.AppliedEvolutions;

        //플레이어 생성
        Player = Instantiate(playerData.PlayerPrefab, spawnPos, Quaternion.identity);

        //플레이어 초기화
        Player.Init(playerData, this);

        //기본 무기 장착
        Player.TryAddWeapon(weaponData);

        //적용된 진화들 장착
        foreach (var evolutionEntry in appliedEvolutions)
        {
            var evolutionData = evolutionEntry.Key;
            var evolutionLevel = evolutionEntry.Value;
            Player.AddEvolution(evolutionData, evolutionLevel);
        }

        //플레이어 체력 최대로 설정
        Player.HealFull();

        //스폰 애니메이션 재생
        Player.OnSpawn();
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

    //라운드 데이터 초기화
    private void InitRoundData()
    {
        //런 데이터 가져오기
        var runData = GlobalGameManager.Instance.SelectedRunData;

        //테이블 수를 타겟 라운드로 설정
        TargetRound = runData.EnemyTableDataList.EnemyTableDatas.Count;

        //현재 라운드를 0으로 초기화
        CurrentRound = 0;
    }
    #endregion

    private void Update()
    {
        StateMachine.Update();
    }
}