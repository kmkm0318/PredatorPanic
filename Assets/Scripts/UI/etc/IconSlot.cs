using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이콘 슬롯 클래스
/// 아이콘을 표시하는 UI 슬롯을 나타냄
/// 백그라운드 색을 변경할 수 있고 아이콘 이미지를 변경할 수 있음
/// </summary>
public class IconSlot : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _iconImage;

    /// <summary>
    /// 배경 색 설정
    /// </summary>
    public void SetColor(Color color)
    {
        _background.color = color;
    }

    /// <summary>
    /// 아이콘 이미지 설정
    /// </summary>
    public void SetIcon(Sprite icon)
    {
        _iconImage.sprite = icon;
    }
}