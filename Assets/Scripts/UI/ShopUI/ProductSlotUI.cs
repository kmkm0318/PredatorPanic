using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 상품 슬롯 UI 클래스
/// 상점에서 상품을 구매하거나 판매할 때 사용하는 슬롯
/// </summary>
public class ProductSlotUI : MonoBehaviour
{
    [Header("Product Slot UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private PointerHandler _PointerHandler;

    #region 타겟 상품
    private IProduct _product;
    #endregion

    #region 이벤트
    private event Action<IProduct> _onClicked;
    #endregion

    private void Awake()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        //클릭 이벤트 등록
        _PointerHandler.OnPointerClicked += OnClicked;
    }

    private void OnClicked()
    {
        _onClicked?.Invoke(_product);
    }

    /// <summary>
    /// 상점 상품 슬롯 초기화 함수
    /// </summary>
    public void Init(IProduct product, Action<IProduct> onClicked)
    {
        //데이터 설정
        _product = product;
        _onClicked = onClicked;

        //UI 업데이트
        _icon.sprite = product.Icon;
        _nameText.text = product.Name;
        _priceText.text = product.Price.ToString();
    }
}