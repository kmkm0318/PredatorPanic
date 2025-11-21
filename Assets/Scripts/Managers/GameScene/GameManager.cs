using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 게임 씬에서 사용하는 매니저 클래스
/// 플레이어와 카메라를 초기화
/// 플레이어에게 기본 무기 장착
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [Header("Enemy")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private int _initialEnemyCount = 10;

    [Header("DropItem")]
    [SerializeField] private DropItemManager _dropItemManager;

    [Header("UI")]
    [SerializeField] private GameUIManager _gameUIManager;

    public Player Player { get; private set; }

    private void Start()
    {
        Init();
        StartGame();
    }

    //초기화
    private void Init()
    {
        InitPlayer();
        InitCameraTarget();
        _gameUIManager.Init(Player);
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

    // 게임 시작
    private void StartGame()
    {
        SpawnEnemies();
        InputManager.Instance.EnablePlayerInput();
    }

    private void SpawnEnemies()
    {
        _enemyManager.SpawnEnemies(Player.transform, _initialEnemyCount);
    }
}