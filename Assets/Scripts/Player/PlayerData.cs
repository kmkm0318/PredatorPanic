using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터 스크립터블 오브젝트
/// 플레이어 프리팹, 비주얼 프리팹, 컨트롤러 데이터 포함
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Header("Player Prefab")]
    [SerializeField] private Player _playerPrefab;
    public Player PlayerPrefab => _playerPrefab;

    [Header("Player Controller Data")]
    [SerializeField] private PlayerControllerData _playerControllerData;
    public PlayerControllerData PlayerControllerData => _playerControllerData;

    [Header("Stats")]
    [SerializeField] private List<StatEntity<PlayerStatType>> _initialStats;
    public List<StatEntity<PlayerStatType>> InitialStats => _initialStats;

    [Header("EXP")]
    [SerializeField] private float _baseExp = 100f;
    [SerializeField] private float _expGrowthRate = .25f;
    public float BaseExp => _baseExp;
    public float ExpGrowthRate => _expGrowthRate;

    [Header("Weapon")]
    [SerializeField] private int _weaponCountMax = 6;
    public int WeaponCountMax => _weaponCountMax;
}