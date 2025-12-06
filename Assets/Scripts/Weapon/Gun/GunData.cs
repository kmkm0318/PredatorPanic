using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총 무기 데이터 스크립터블 오브젝트. WeaponData 상속.
/// 총의 기본 정보, 사격 정보, 탄환 궤적 정보 포함
/// </summary>
[CreateAssetMenu(fileName = "GunData", menuName = "SO/Weapon/Gun/GunData", order = 0)]
public class GunData : WeaponData
{
    [Header("Basic Data")]
    [SerializeField] private GunType _type;
    [SerializeField] private string _name;
    public GunType Type => _type;
    public string Name => _name;

    [Header("Fire Data")]
    // [SerializeField] private bool _isHitScan = true; //히트스캔 사용하지 않음
    [SerializeField] private BulletData _bulletData;
    [SerializeField] private TrailData _trailData;
    [SerializeField] private ExplosionData _explosionData;
    [SerializeField] private LayerMask _hitLayerMask;
    // public bool IsHitScan => _isHitScan; //히트스캔 사용하지 않음
    public BulletData BulletData => _bulletData;
    public TrailData TrailData => _trailData;
    public ExplosionData ExplosionData => _explosionData;
    public LayerMask HitLayerMask => _hitLayerMask;

    [Header("Stats")]
    [SerializeField] private List<StatEntity<GunStatType>> _initialStats;
    public List<StatEntity<GunStatType>> InitialStats => _initialStats;
}