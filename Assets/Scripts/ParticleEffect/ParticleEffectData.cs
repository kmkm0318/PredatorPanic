using UnityEngine;

[CreateAssetMenu(fileName = "ParticleEffectData", menuName = "SO/ParticleEffect/ParticleEffectData", order = 0)]
public class ParticleEffectData : ScriptableObject
{
    [Header("Prefab")]
    [SerializeField] private ParticleEffect _particleEffectPrefab;
    public ParticleEffect ParticleEffectPrefab => _particleEffectPrefab;
}