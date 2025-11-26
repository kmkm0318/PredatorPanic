using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 아이템 클래스
/// </summary>
public class Item
{
    //데이터
    public ItemData ItemData { get; private set; }

    //효과 리스트
    private List<Effect> _effects = new();

    //생성자.
    //효과들을 리스트로 생성
    public Item(ItemData itemData)
    {
        ItemData = itemData;

        foreach (var effectData in ItemData.EffectDatas)
        {
            var effect = effectData.GetEffect();
            _effects.Add(effect);
        }
    }

    //설명 반환
    public string GetDescription()
    {
        return string.Join("\n", _effects.Select(effect => effect.GetDescription()));
    }

    /// <summary>
    /// 아이템 장착 시 효과 적용
    /// </summary>
    public void OnEquip(Player player)
    {
        foreach (var effect in _effects)
        {
            effect.Apply(player);
        }
    }

    /// <summary>
    /// 아이템 장착 해제 시 효과 제거
    /// </summary>
    public void OnUnequip(Player player)
    {
        foreach (var effect in _effects)
        {
            effect.Remove(player);
        }
    }
}