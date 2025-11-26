/// <summary>
/// 아이템 효과 클래스
/// </summary>
public class ItemEffect
{
    //데이터
    public ItemEffectData ItemEffectData { get; private set; }

    //ItemEffectData에서 ItemEffect를 생성하고 반환합니다.
    public ItemEffect(ItemEffectData itemEffectData)
    {
        ItemEffectData = itemEffectData;
    }

    /// <summary>
    /// 아이템 장착 시 호출되는 함수
    /// </summary>
    public virtual void OnEquip(Player player)
    {
        //기본 구현은 없음. 서브클래스에서 오버라이드하여 장착 시 효과 적용 로직 구현
    }

    /// <summary>
    /// 아이템 해제 시 호출되는 함수
    /// </summary>
    public virtual void OnUnequip(Player player)
    {
        //기본 구현은 없음. 서브클래스에서 오버라이드하여 해제 시 효과 적용 로직 구현
    }
}