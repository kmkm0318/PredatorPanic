/// <summary>
/// 아이템 클래스
/// </summary>
public class Item
{
    public ItemData ItemData { get; private set; }

    public Item(ItemData itemData)
    {
        ItemData = itemData;
    }
}