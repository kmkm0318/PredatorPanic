using UnityEngine;

/// <summary>
/// 공격 적중 시 퍼센트 데미지 효과 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "HealthBasedDamageOnHitEffectData", menuName = "SO/Effect/HealthBasedDamageOnHitEffectData", order = 0)]
public class HealthBasedDamageOnHitEffectData : EffectData
{
    [Header("Percent Damage On Hit Info")]
    [SerializeField, Range(0f, 1f)] private float _damageRate = 0.01f;
    [SerializeField] private bool _isCurrentHealthBased = false;
    public float DamageRate => _damageRate;
    public bool IsCurrentHealthBased => _isCurrentHealthBased;

    public override Effect GetEffect()
    {
        return new HealthBasedDamageOnHitEffect(this);
    }
    public override string GetDescription()
    {
        string healthType = _isCurrentHealthBased ? "현재 체력" : "최대 체력";
        return $"적중 시 대상의 {healthType}의 {_damageRate * 100f}% 데미지를 추가로 입힘";
    }
}
