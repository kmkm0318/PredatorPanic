using UnityEngine;

/// <summary>
/// 총 무기 데이터 스크립터블 오브젝트
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

    [Header("Shoot Data")]
    [SerializeField] private LayerMask _hitLayerMask;
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private float _spread = 0.1f;
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _damage;
    public LayerMask HitLayerMask => _hitLayerMask;
    public float FireRate => _fireRate;
    public float Spread => _spread;
    public float Range => _range;
    public float Damage => _damage;

    [Header("Trail Data")]
    [SerializeField] private TrailRenderer _trailRendererPrefab;

    [SerializeField] private float _speed = 100f;
    public TrailRenderer TrailRendererPrefab => _trailRendererPrefab;
    public float Speed => _speed;
}