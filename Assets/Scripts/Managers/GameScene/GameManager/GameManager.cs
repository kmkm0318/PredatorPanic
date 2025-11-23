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
    [SerializeField] private GameManagerData _gameManagerData;
    public GameManagerData GameManagerData => _gameManagerData;

    [Header("Player")]
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [Header("Managers")]
    [SerializeField] private EnemyManager _enemyManager;
    public EnemyManager EnemyManager => _enemyManager;
    [SerializeField] private DropItemManager _dropItemManager;
    public DropItemManager DropItemManager => _dropItemManager;
    [SerializeField] private GameUIManager _gameUIManager;
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
        _gameUIManager.Init(Player);
        CurrentRound = 1;
    }

    // 플레이어 생성 및 초기화
    private void InitPlayer()
    {
        Player = Instantiate(_playerData.PlayerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation);
        Player.Init(_playerData, _weaponData);
    }

    // 시네머신 카메라의 팔로우 타겟 설정
    private void InitCameraTarget()
    {
        if (_cinemachineCamera != null)
        {
            Player.SetCameraFollowTarget(_cinemachineCamera);
        }
    }
    #endregion

    private void Update()
    {
        StateMachine.Update();
    }
}