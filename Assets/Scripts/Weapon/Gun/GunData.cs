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
    [SerializeField] private bool _isHitScan = true;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private LayerMask _hitLayerMask;
    public bool IsHitScan => _isHitScan;
    public Bullet BulletPrefab => _bulletPrefab;
    public LayerMask HitLayerMask => _hitLayerMask;

    [Header("Trail Renderer Prefab")]
    [SerializeField] private TrailRenderer _trailRendererPrefab;
    public TrailRenderer TrailRendererPrefab => _trailRendererPrefab;

    [Header("Stats")]
    [SerializeField] private List<StatEntity<GunStatType>> _initialStats;
    public List<StatEntity<GunStatType>> InitialStats => _initialStats;
}