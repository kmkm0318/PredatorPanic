using UnityEngine;

/// <summary>
/// 이빨 아이템
/// 플레이어가 획득 시 이빨 재화 획득
/// </summary>
public class Tooth : DropItem
{
    [SerializeField] private int _toothAmount = 10;

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);

        player.AddTooth(_toothAmount);
    }
}