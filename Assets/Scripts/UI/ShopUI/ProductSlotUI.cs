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
    [Header("Product Slot UI")]
    [SerializeField] private IconSlot _iconSlot;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private PointerHandler _PointerHandler;

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

    private void RegisterEvents()
    {
        //클릭 이벤트 등록
        _PointerHandler.OnPointerClicked += () => OnClicked?.Invoke(_product);

        //포인터 진입 및 이탈 이벤트 등록
        _PointerHandler.OnPointerEntered += () => OnPointerEntered?.Invoke(_product);
        _PointerHandler.OnPointerExited += () => OnPointerExited?.Invoke(_product);
    }

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