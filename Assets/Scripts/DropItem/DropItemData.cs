using UnityEngine;

/// <summary>
/// 드롭 아이템 데이터
/// DropRate에 따라 아이템을 드롭
/// MinDropCount와 MaxDropCount 사이의 개수를 드롭
/// </summary>
[CreateAssetMenu(fileName = "DropItemData", menuName = "SO/DropItem/DropItemData", order = 0)]
public class DropItemData : ScriptableObject
{
    [Header("Item Prefab")]
    [SerializeField] private DropItem _itemPrefab;
    public DropItem ItemPrefab => _itemPrefab;

    [Header("Drop Settings")]
    [SerializeField][Range(0f, 1f)] private float _dropRate = 1f;
    [SerializeField] private int _minDropCount = 1;
    [SerializeField] private int _maxDropCount = 1;
    [SerializeField] private float _dropRadius = 1f;
    public float DropRate => _dropRate;
    public int MinDropCount => _minDropCount;
    public int MaxDropCount => _maxDropCount;
    public float DropRadius => _dropRadius;

    [Header("Follow Settings")]
    [SerializeField] private bool _isFollow = true;
    [SerializeField] private float _followHeight = 2f;
    [SerializeField] private float _followSpeed = 20f;
    [SerializeField] private float _targetHeight = 1f;
    public bool IsFollow => _isFollow;
    public float FollowHeight => _followHeight;
    public float FollowSpeed => _followSpeed;
    public float TargetHeight => _targetHeight;
}