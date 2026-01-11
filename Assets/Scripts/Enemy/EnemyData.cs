using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 적 데이터 스크립터블 오브젝트 클래스
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    [Header("Basic Data")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private bool _isBoss = false;
    [SerializeField] private string _enemyName;
    public Enemy EnemyPrefab => _enemyPrefab;
    public bool IsBoss => _isBoss;
    public string EnemyName => _enemyName;

    [Header("Controller Data")]
    [SerializeField] private EnemyControllerData _enemyControllerData;
    public EnemyControllerData EnemyControllerData => _enemyControllerData;

    [Header("Attack Data")]
    [SerializeField] private List<EnemyAttackPatternData> _attackPatterns;
    public List<EnemyAttackPatternData> AttackPatterns => _attackPatterns;

    [Header("Stats")]
    [SerializeField] private List<StatEntity<EnemyStatType>> _baseStats;
    public List<StatEntity<EnemyStatType>> BaseStats => _baseStats;

    [Header("Drop Item Table")]
    [SerializeField] private DropItemTableData _dropItemTable;
    public DropItemTableData DropItemTable => _dropItemTable;

    [Header("Audio Data")]
    [SerializeField] private AudioData _hitAudio;
    [SerializeField] private AudioData _deathAudio;
    public AudioData HitAudio => _hitAudio;
    public AudioData DeathAudio => _deathAudio;

    [Header("Particle Effect Data")]
    [SerializeField] private ParticleEffectData _hitParticleEffect;
    [SerializeField] private ParticleEffectData _deathParticleEffect;
    public ParticleEffectData HitParticleEffect => _hitParticleEffect;
    public ParticleEffectData DeathParticleEffect => _deathParticleEffect;

    [Header("Spawn Animation Data")]
    [SerializeField] private float _spawnVisualOffsetY = -10f;
    [SerializeField] private float _spawnVisualDuration = 0.5f;
    [SerializeField] private Ease _spawnVisualEase = Ease.OutBack;
    public float SpawnVisualOffsetY => _spawnVisualOffsetY;
    public float SpawnVisualDuration => _spawnVisualDuration;
    public Ease SpawnVisualEase => _spawnVisualEase;
}