using UnityEngine;

/// <summary>
/// 킬 시 회복 효과 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "HealOnKillEffectData", menuName = "SO/Effect/HealOnKillEffectData", order = 0)]
public class HealOnKillEffectData : EffectData
{
    [Header("Heal On Kill Info")]
    [SerializeField] private int _targetKillCount = 1;
    [SerializeField] private float _healAmount = 1f;
    public int TargetKillCount => _targetKillCount;
    public float HealAmount => _healAmount;

    public override Effect GetEffect()
    {
        return new HealOnKillEffect(this);
    }

    public override string GetDescription()
    {
        if (_targetKillCount <= 1)
        {
            //목표 처치 수가 1 이하일 시 targetKillCount 포함하지 않음
            return $"적 처치 시 {_healAmount} 회복";
        }
        else
        {
            //목표 처치 수가 2 이상일 시 targetKillCount 포함
            return $"적 {_targetKillCount}회 처치 시 {_healAmount} 회복";
        }
    }
}
