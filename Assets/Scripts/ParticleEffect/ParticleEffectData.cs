using UnityEngine;

/// <summary>
/// 파티클 이펙트 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "ParticleEffectData", menuName = "SO/ParticleEffect/ParticleEffectData", order = 0)]
public class ParticleEffectData : ScriptableObject
{
    [Header("Prefab")]
    [SerializeField] private ParticleEffect _particleEffectPrefab;
    public ParticleEffect ParticleEffectPrefab => _particleEffectPrefab;
}