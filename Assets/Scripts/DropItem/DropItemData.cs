using UnityEngine;

/// <summary>
/// 드롭 아이템 데이터
/// 드롭 아이템의 프리팹과 데이터를 담고 있는 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "DropItemData", menuName = "SO/DropItem/DropItemData", order = 0)]
public class DropItemData : ScriptableObject
{
    [Header("Item Prefab")]
    [SerializeField] private DropItem _itemPrefab;
    public DropItem ItemPrefab => _itemPrefab;

    [Header("Drop Settings")]
    [SerializeField] private float _dropRadius = 1f;
    public float DropRadius => _dropRadius;

    [Header("Follow Settings")]
    [SerializeField] private bool _isFollow = true;
    [SerializeField] private float _initialFollowSpeed = 10f;
    [SerializeField] private float _followSpeedAcceleration = 10f;
    public bool IsFollow => _isFollow;
    public float InitialFollowSpeed => _initialFollowSpeed;
    public float FollowSpeedAcceleration => _followSpeedAcceleration;

    [Header("Audio Settings")]
    [SerializeField] private AudioData _pickupSfxData;
    public AudioData PickupSfxData => _pickupSfxData;
}