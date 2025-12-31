using UnityEngine;

/// <summary>
/// 총알 데이터 클래스
/// </summary>
[CreateAssetMenu(fileName = "BulletData", menuName = "SO/Projectile/BulletData", order = 0)]
public class BulletData : ScriptableObject
{
    [Header("Prefab")]
    [SerializeField] private Bullet _bulletPrefab;
    public Bullet BulletPrefab => _bulletPrefab;

    [Header("Homing")]
    [SerializeField] private bool _isHoming = false;
    [SerializeField] private float _homingPower = 15f;
    [SerializeField] private float _homingDelay = 0.2f;
    public bool IsHoming => _isHoming;
    public float HomingPower => _homingPower;
    public float HomingDelay => _homingDelay;
}