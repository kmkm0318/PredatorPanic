using System;
using UnityEngine;

/// <summary>
/// 공격 적중 시 추가 탄환 발사 효과 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "AdditionalBulletOnHitEffectData", menuName = "SO/Effect/AdditionalBulletOnHitEffectData", order = 0)]
public class AdditionalBulletOnHitEffectData : EffectData
{
    [Header("Additional Bullet On Hit Info")]
    [SerializeField, Range(0f, 1f)] private float _chance = 1f;
    [SerializeField] private BulletData _bulletData;
    [SerializeField] private TrailData _trailData;

    public float Chance => _chance;
    public BulletData BulletData => _bulletData;
    public TrailData TrailData => _trailData;

    [Header("Bullet Data")]
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _range = 25f;
    [SerializeField] private float _speed = 50f;

    public float Damage => _damage;
    public float Range => _range;
    public float Speed => _speed;

    public override Effect GetEffect()
    {
        return new AdditionalBulletOnHitEffect(this);
    }

    public override string GetDescription()
    {
        if (_chance >= 1f)
        {
            return $"적중 시 추가 탄환 발사";
        }
        else
        {
            return $"적중 시 {_chance * 100f}% 확률로 추가 탄환 발사";
        }
    }
}
