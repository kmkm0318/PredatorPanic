using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    [Header("Basic Data")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private string _enemyName;
    [SerializeField] private float _maxHealth;
    public Enemy EnemyPrefab => _enemyPrefab;
    public string EnemyName => _enemyName;
    public float MaxHealth => _maxHealth;

    [Header("Controller Data")]
    [SerializeField] private EnemyControllerData _enemyControllerData;
    public EnemyControllerData EnemyControllerData => _enemyControllerData;
}