using UnityEngine;

/// <summary>
/// 폭발 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "ExplosionData", menuName = "SO/Projectile/ExplosionData", order = 0)]
public class ExplosionData : ScriptableObject
{
    [Header("Prefab")]
    [field: SerializeField] public Explosion ExplosionPrefab { get; private set; }

    [Header("Explosion Settings")]
    [field: SerializeField] public float VisualDuration { get; private set; } = 0.1f;

    [Header("Sfx Settings")]
    [SerializeField] private AudioData _explodeSfxData;
    public AudioData ExplodeSfxData => _explodeSfxData;
}