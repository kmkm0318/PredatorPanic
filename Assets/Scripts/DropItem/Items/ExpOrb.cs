using UnityEngine;

/// <summary>
/// 경험치 오브
/// </summary>
public class ExpOrb : DropItem
{
    [SerializeField] private float _expAmount = 10f;

    public override void OnPickup(Player player)
    {
        base.OnPickup(player);

        player.AddExp(_expAmount);
    }
}