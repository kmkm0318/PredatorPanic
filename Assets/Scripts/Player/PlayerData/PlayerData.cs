using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 플레이어 데이터 스크립터블 오브젝트
/// 플레이어 프리팹, 비주얼 프리팹, 컨트롤러 데이터 포함
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = 0)]
public class PlayerData : ScriptableObject, IBasicData
{
    [Header("Basic Info")]
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private int _basePrice = 500;
    public string ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public Rarity Rarity => _rarity;
    public int BasePrice => _basePrice;

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

    public string GetDescription()
    {
        //스트링 리스트 생성
        List<string> effecDescriptions = new();

        //각 이펙트 데이터의 설명을 스트링 리스트에 추가
        foreach (var effectData in _characterEffectDatas)
        {
            effecDescriptions.Add(effectData.GetDescription());
        }

        //개행 문자로 구분된 설명 반환
        return string.Join("\n", effecDescriptions);
    }
}