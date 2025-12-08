using UnityEngine;

/// <summary>
/// 체력 아이템
/// 플레이어가 획득 시 체력 회복
/// </summary>
public class Heart : DropItem
{
    [SerializeField] private float _healAmount = 10f;

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);

        player.PlayerHealth.Heal(_healAmount);
    }
}