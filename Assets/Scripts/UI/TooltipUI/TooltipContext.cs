using UnityEngine;

/// <summary>
/// 툴팁 컨텍스트 클래스
/// </summary>
public class TooltipContext
{
    public object Target { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Color Color { get; private set; } = Color.white;
    public Sprite Icon { get; private set; } = null;
    public int Price { get; private set; } = 0;

    public TooltipContext(object target, string name, string description, Color color = default, Sprite icon = null, int price = 0)
    {
        Target = target;
        Name = name;
        Description = description;
        Color = color == default ? Color.white : color;
        Icon = icon;
        Price = price;
    }
}