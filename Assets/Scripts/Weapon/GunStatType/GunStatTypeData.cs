using UnityEngine;

/// <summary>
/// 총기 스탯 타입 데이터 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "GunStatTypeData", menuName = "SO/Weapon/Gun/GunStatTypeData", order = 0)]
public class GunStatTypeData : ScriptableObject
{
    [Header("Gun Stat Type Data")]
    [SerializeField] private GunStatType _gunStatType;
    [SerializeField] private string _statName;
    [SerializeField] private string _statDescription;
    public GunStatType GunStatType => _gunStatType;
    public string StatName => _statName;
    public string StatDescription => _statDescription;
}