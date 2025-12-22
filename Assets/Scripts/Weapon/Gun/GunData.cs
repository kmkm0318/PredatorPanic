using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총 무기 데이터 스크립터블 오브젝트. WeaponData 상속.
/// 총의 기본 정보, 사격 정보, 탄환 궤적 정보 포함
/// </summary>
[CreateAssetMenu(fileName = "GunData", menuName = "SO/Weapon/Gun/GunData", order = 0)]
public class GunData : WeaponData
{
    [Header("Basic Gun Data")]
    [SerializeField] private GunType _type;
    public GunType Type => _type;

    [Header("Gun Fire Data")]
    [SerializeField] private BulletData _bulletData;
    [SerializeField] private TrailData _trailData;
    [SerializeField] private ExplosionData _explosionData;
    [SerializeField] private AudioData _fireSfxData;
    public BulletData BulletData => _bulletData;
    public TrailData TrailData => _trailData;
    public ExplosionData ExplosionData => _explosionData;
    public AudioData FireSfxData => _fireSfxData;

    [Header("Gun Stats")]
    [SerializeField] private List<StatEntity<GunStatType>> _initialStats;
    public List<StatEntity<GunStatType>> InitialStats => _initialStats;

    public override Weapon GetWeapon()
    {
        return new Gun(this);
    }
}