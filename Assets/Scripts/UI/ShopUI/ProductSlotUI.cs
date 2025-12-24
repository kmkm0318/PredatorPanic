using System;
using TMPro;
using UnityEngine;

/// <summary>
/// 상품 슬롯 UI 클래스
/// 상점에서 상품을 구매하거나 판매할 때 사용하는 슬롯
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ProductSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private IconSlot _iconSlot;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private PointerHandler _pointerHandler;

    #region 타겟 상품
    private IProduct _product;
    #endregion

    #region 이벤트
    public event Action<IProduct> OnClicked;
    public event Action<IProduct> OnPointerEntered;
    public event Action<IProduct> OnPointerExited;
    #endregion

    private void Awake()
    {
        RegisterEvents();
    }

    #region 이벤트 등록
    private void RegisterEvents()
    {
        _pointerHandler.OnPointerEntered += HandleOnPointerEntered;
        _pointerHandler.OnPointerExited += HandleOnPointerExited;
        _pointerHandler.OnPointerClicked += HandleOnPointerClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnPointerEntered()
    {
        //이벤트 호출
        OnPointerEntered?.Invoke(_product);
    }

    private void HandleOnPointerExited()
    {
        //이벤트 호출
        OnPointerExited?.Invoke(_product);
    }

    private void HandleOnPointerClicked()
    {
        //이벤트 호출
        OnClicked?.Invoke(_product);
    }
    #endregion

    /// <summary>
    /// 상점 상품 슬롯 초기화 함수
    /// </summary>
    public void Init(IProduct product)
    {
        //데이터 설정
        _product = product;

        //UI 업데이트
        if (_iconSlot)
        {
            //레어도 색 가져오기
            if (DataManager.Instance.RarityDataList.RarityDataDict.TryGetValue(product.Rarity, out var rarityData))
            {
                //배경 색 설정
                _iconSlot.SetColor(rarityData.RarityColor);

                if (_nameText)
                {
                    //이름 색 설정
                    _nameText.color = rarityData.RarityColor;
                }
            }

            //아이콘 설정
            _iconSlot.SetIcon(product.Icon);
        }

        //이름 설정
        if (_nameText)
        {
            _nameText.text = product.Name;
        }

        //가격 설정
        if (_priceText)
        {
            _priceText.text = product.Price.ToString();
        }
    }
}