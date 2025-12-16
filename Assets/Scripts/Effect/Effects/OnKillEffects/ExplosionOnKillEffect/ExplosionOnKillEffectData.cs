using UnityEngine;

/// <summary>
/// 적 처치 시 폭발 효과 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "ExplosionOnKillEffectData", menuName = "SO/Effect/ExplosionOnKillEffectData", order = 0)]
public class ExplosionOnKillEffectData : EffectData
{
    [Header("Explosion On Kill Info")]
    [SerializeField] private ExplosionData _explosionData;
    [SerializeField, Range(0f, 1f)] private float _chance = 1f;
    public ExplosionData ExplosionData => _explosionData;
    public float Chance => _chance;

    [Header("Explosion Data")]
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _radius = 5f;
    public float Damage => _damage;
    public float Radius => _radius;

    public override Effect GetEffect()
    {
        return new ExplosionOnKillEffect(this);
    }

    public override string GetDescription()
    {
        if (_chance >= 1f)
        {
            return $"적 처치 시 폭발 발생";
        }
        else
        {
            return $"적 처치 시 {_chance * 100f}% 확률로 폭발 발생";
        }
    }
}
