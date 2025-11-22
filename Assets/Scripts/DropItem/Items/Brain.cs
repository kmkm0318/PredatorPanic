using UnityEngine;

/// <summary>
/// 브레인 아이템
/// 플레이어가 획득 시 경험치 획득
/// </summary>
public class Brain : DropItem
{
    [SerializeField] private float _expAmount = 10f;

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);

        player.AddExp(_expAmount);
    }
}