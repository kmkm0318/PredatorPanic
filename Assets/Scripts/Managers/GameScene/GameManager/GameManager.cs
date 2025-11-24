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
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [Header("Managers")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private DropItemManager _dropItemManager;
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private GameUIManager _gameUIManager;
    public EnemyManager EnemyManager => _enemyManager;
    public DropItemManager DropItemManager => _dropItemManager;
    public ShopManager ShopManager => _shopManager;
    public GameUIManager GameUIManager => _gameUIManager;

    #region 플레이어
    public Player Player { get; private set; }
    #endregion

    #region 상태기계
    public StateMachine StateMachine { get; private set; }
    private GameStateFactory stateFactory;
    #endregion

    #region 게임 데이터
    public int CurrentRound { get; private set; }
    public float RoundTimer { get; set; }
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
        InitCameraTarget();
        InitManagers();
        CurrentRound = 1;
    }

    // 플레이어 생성 및 초기화
    private void InitPlayer()
    {
        Player = Instantiate(_playerData.PlayerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
        Player.Init(_playerData);
        Player.TryAddWeapon(_weaponData);
    }

    // 시네머신 카메라의 팔로우 타겟 설정
    private void InitCameraTarget()
    {
        if (_cinemachineCamera != null)
        {
            Player.SetCameraFollowTarget(_cinemachineCamera);
        }
    }

    //매니저 클래스 초기화. UI는 마지막에 초기화
    private void InitManagers()
    {
        _shopManager.Init(Player);
        _gameUIManager.Init(this);
    }
    #endregion

    private void Update()
    {
        StateMachine.Update();
    }
}