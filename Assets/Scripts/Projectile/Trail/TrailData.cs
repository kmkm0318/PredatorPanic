using UnityEngine;

[CreateAssetMenu(fileName = "TrailData", menuName = "SO/Projectile/TrailData", order = 0)]
public class TrailData : ScriptableObject
{
    [Header("Prefab")]
    [field: SerializeField] public Trail TrailPrefab { get; private set; }
}