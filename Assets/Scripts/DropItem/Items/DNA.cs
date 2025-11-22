using UnityEngine;

/// <summary>
/// DNA 아이템
/// 플레이어가 획득 시 DNA 재화 획득
/// </summary>
public class DNA : DropItem
{
    [SerializeField] private int _dnaAmount = 10;

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);

        player.AddDNA(_dnaAmount);
    }
}