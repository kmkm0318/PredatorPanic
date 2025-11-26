using UnityEngine;

/// <summary>
/// 총알 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "BulletData", menuName = "SO/Projectile/BulletData", order = 0)]
public class BulletData : ScriptableObject
{
    [Header("Prefab")]
    [field: SerializeField] public Bullet BulletPrefab { get; private set; }
}