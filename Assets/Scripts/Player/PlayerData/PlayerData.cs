using System;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private PlayerBaseStatsData _playerBaseStatsData;
    [SerializeField] private List<EffectData> _characterEffectDatas;
    public PlayerBaseStatsData PlayerBaseStatsData => _playerBaseStatsData;
    public List<EffectData> CharacterEffectDatas => _characterEffectDatas;

    [Header("EXP")]
    [SerializeField] private PlayerExpData _playerExpData;
    public PlayerExpData PlayerExpData => _playerExpData;

    [Header("Weapon")]
    [SerializeField] private int _weaponCountMax = 6;
    public int WeaponCountMax => _weaponCountMax;

    [Header("Item Pickup")]
    [SerializeField] private float _itemPickupRadiusSqr = 4f;
    public float ItemPickupRadiusSqr => _itemPickupRadiusSqr;

    [Header("Spawn Animation Data")]
    [SerializeField] private float _spawnVisualOffsetY = -10f;
    [SerializeField] private float _spawnVisualDuration = 0.5f;
    [SerializeField] private Ease _spawnVisualEase = Ease.OutBack;
    public float SpawnVisualOffsetY => _spawnVisualOffsetY;
    public float SpawnVisualDuration => _spawnVisualDuration;
    public Ease SpawnVisualEase => _spawnVisualEase;

    [Header("Audio Data")]
    [SerializeField] private AudioData _hitAudioData;
    [SerializeField] private AudioData _deathAudioData;
    public AudioData HitAudioData => _hitAudioData;
    public AudioData DeathAudioData => _deathAudioData;

    [Header("Particle Effect Data")]
    [SerializeField] private ParticleEffectData _hitParticleEffectData;
    [SerializeField] private ParticleEffectData _deathParticleEffectData;
    public ParticleEffectData HitParticleEffectData => _hitParticleEffectData;
    public ParticleEffectData DeathParticleEffectData => _deathParticleEffectData;
}