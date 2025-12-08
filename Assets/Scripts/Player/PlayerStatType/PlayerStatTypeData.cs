using UnityEngine;

/// <summary>
/// 플레이어 스탯 타입 데이터
/// 이름, 설명 등을 반환하기 위해서 사용합니다.
/// </summary>
[CreateAssetMenu(fileName = "PlayerStatTypeData", menuName = "SO/Player/PlayerStatTypeData", order = 0)]
public class PlayerStatTypeData : ScriptableObject
{
    [Header("Player Stat Type Info")]
    [SerializeField] private PlayerStatType _playerStatType;
    [SerializeField] private string _statName;
    [SerializeField] private string _statDescription;
    public PlayerStatType PlayerStatType => _playerStatType;
    public string StatName => _statName;
    public string StatDescription => _statDescription;
}