using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 툴팁 UI 클래스
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class TooltipUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private float _panelOffset = 5f;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _priceText;

    private void Update()
    {
        HandlePanelPosition();
    }

    #region 패널 위치 조절
    //툴팁 패널 위치 조절 함수
    private void HandlePanelPosition()
    {
        //마우스 위치 가져오기
        var mousePos = InputManager.Instance.MousePosition;

        //UI 위치 설정
        HandleUIPosition(mousePos);

        //패널 피벗 조정
        HandlePanelPivot(mousePos);

        //패널 오프셋 조정
        HandlePanelOffset();
    }

    //UI 위치 설정 함수
    private void HandleUIPosition(Vector2 mousePos)
    {
        //마우스 위치를 그대로 지정
        _rectTransform.position = mousePos;
    }

    //패널 피벗 조절 함수
    private void HandlePanelPivot(Vector2 mousePos)
    {
        //마우스 위치에 따라서 피벗 조절. 스크린의 절반을 기준으로 크면 1, 작으면 0
        var pivotX = mousePos.x > Screen.width / 2 ? 1f : 0f;
        var pivotY = mousePos.y > Screen.height / 2 ? 1f : 0f;

        //피벗 설정
        _panel.pivot = new Vector2(pivotX, pivotY);
    }

    //피벗에 따른 패널 offset 설정
    private void HandlePanelOffset()
    {
        //피벗에 따른 오프셋 설정. 피벗과 반대 방향으로 offset만큼 이동
        Vector2 offset = new()
        {
            x = _panel.pivot.x == 1 ? -_panelOffset : _panelOffset,
            y = _panel.pivot.y == 1 ? -_panelOffset : _panelOffset
        };

        _panel.anchoredPosition = offset;
    }
    #endregion

    #region Show, Hide
    public void Show(IProduct product)
    {
        //데이터 설정
        _icon.sprite = product.Icon;
        _nameText.text = product.Name;
        _descriptionText.text = product.Description;
        _priceText.text = product.Price.ToString();

        //패널 활성화
        _panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        //패널 비활성화
        _panel.gameObject.SetActive(false);
    }
    #endregion
}