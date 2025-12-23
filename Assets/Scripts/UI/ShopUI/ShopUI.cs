using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// 상점 UI 클래스
/// 상점에서 상품을 구매하거나 플레이어의 무기, 아이템을 판매할 수 있습니다
/// 판매 아이템을 새로고침하거나 다음 라운드로 넘어갈 수 있습니다
/// </summary>
public class ShopUI : ShowHideUI
{
    [Header("Product Prefab and Parents")]
    [SerializeField] private ProductSlotUI _shopProductSlotUIPrefab;
    [SerializeField] private ProductSlotUI _inventoryProductSlotUIPrefab;
    [SerializeField] private Transform _shopProductParent;
    [SerializeField] private Transform _weaponInventoryProductParent;
    [SerializeField] private Transform _itemInventoryProductParent;

    [Header("Refresh")]
    [SerializeField] private PointerHandler _refreshButton;
    [SerializeField] private TMP_Text _refreshCostText;

    [Header("Next Round")]
    [SerializeField] private PointerHandler _nextRoundButton;

    [Header("Audio Data")]
    [SerializeField] private AudioData _buttonHoverAudioData;
    [SerializeField] private AudioData _buttonClickAudioData;

    #region 오브젝트 풀
    private ObjectPool<ProductSlotUI> _shopProductSlotUIpool;
    private ObjectPool<ProductSlotUI> _inventoryProductSlotUIpool;
    private bool _isPoolInitialized = false;
    #endregion

    #region 이벤트
    public event Action<IProduct> OnShopProductClicked;
    public event Action<IProduct> OnWeaponInventoryProductClicked;
    public event Action<IProduct> OnItemInventoryProductClicked;
    public event Action OnRefreshButtonClicked;
    public event Action OnNextRoundButtonClicked;
    public event Action<IProduct> OnProductPointerEntered;
    public event Action<IProduct> OnProductPointerExited;
    #endregion

    private void Awake()
    {
        InitButtons();
    }

    //버튼 초기화
    private void InitButtons()
    {
        _refreshButton.OnPointerEntered += HandleRefreshButtonEntered;
        _refreshButton.OnPointerClicked += HandleRefreshButtonClicked;

        _nextRoundButton.OnPointerEntered += HandleNextRoundButtonEntered;
        _nextRoundButton.OnPointerClicked += HandleNextRoundButtonClicked;
    }

    private void HandleRefreshButtonEntered()
    {
        //버튼 호버 사운드 재생
        AudioManager.Instance.PlaySfx(_buttonHoverAudioData);
    }

    private void HandleRefreshButtonClicked()
    {
        //버튼 클릭 사운드 재생
        AudioManager.Instance.PlaySfx(_buttonClickAudioData);

        //이벤트 호출
        OnRefreshButtonClicked?.Invoke();
    }

    private void HandleNextRoundButtonEntered()
    {
        //버튼 호버 사운드 재생
        AudioManager.Instance.PlaySfx(_buttonHoverAudioData);
    }

    private void HandleNextRoundButtonClicked()
    {
        //버튼 클릭 사운드 재생
        AudioManager.Instance.PlaySfx(_buttonClickAudioData);

        //이벤트 호출
        OnNextRoundButtonClicked?.Invoke();
    }

    #region 오브젝트 풀링
    private void InitPool()
    {
        _shopProductSlotUIpool = new(
            () => Instantiate(_shopProductSlotUIPrefab),
            (slotUI) => slotUI.gameObject.SetActive(true),
            (slotUI) =>
            {
                slotUI.gameObject.SetActive(false);
                slotUI.transform.SetParent(transform, false);
            },
            (slotUI) => Destroy(slotUI.gameObject),
            false
            );

        _inventoryProductSlotUIpool = new(
            () => Instantiate(_inventoryProductSlotUIPrefab),
            (slotUI) => slotUI.gameObject.SetActive(true),
            (slotUI) =>
            {
                slotUI.gameObject.SetActive(false);
                slotUI.transform.SetParent(transform, false);
            },
            (slotUI) => Destroy(slotUI.gameObject),
            false
            );

        _isPoolInitialized = true;
    }
    #endregion

    #region 상품 설정
    //구매 상품 설정
    public void SetShopProducts(List<IProduct> products)
    {
        if (!_isPoolInitialized) InitPool();
        UpdateProducts(products, _shopProductParent, _shopProductSlotUIpool, HandleShopProductClicked);
    }

    //무기 판매 상품 설정
    public void SetWeaponInventoryProducts(List<WeaponInventoryProduct> products)
    {
        if (!_isPoolInitialized) InitPool();
        UpdateProducts(products, _weaponInventoryProductParent, _inventoryProductSlotUIpool, HandleWeaponInventoryProductClicked);
    }

    //아이템 판매 상품 설정
    public void SetItemInventoryProducts(List<ItemInventoryProduct> products)
    {
        if (!_isPoolInitialized) InitPool();
        UpdateProducts(products, _itemInventoryProductParent, _inventoryProductSlotUIpool, HandleItemInventoryProductClicked);
    }

    //상품 UI 업데이트 공통 함수
    private void UpdateProducts<T>(List<T> products, Transform parent, ObjectPool<ProductSlotUI> pool, Action<IProduct> onClickedHandler) where T : IProduct
    {
        //오브젝트 풀 초기화
        if (pool == null) InitPool();

        //기존 상품 제거
        ClearParent(parent, pool);

        //새 상품 설정
        foreach (var product in products)
        {
            //풀에서 가져오기
            var slotUI = pool.Get();

            //부모 설정 및 마지막으로 이동
            slotUI.transform.SetParent(parent, false);
            slotUI.transform.SetAsLastSibling();

            //초기화
            slotUI.Init(product);

            //이벤트 등록
            slotUI.OnClicked += onClickedHandler;
            slotUI.OnPointerEntered += HandleProductPointerEntered;
            slotUI.OnPointerExited += HandleProductPointerExited;
        }
    }
    #endregion

    //부모 오브젝트의 모든 자식 제거
    //이벤트도 해제하고 오브젝트 풀에 반환
    private void ClearParent(Transform parent, ObjectPool<ProductSlotUI> pool)
    {
        //뒤에서부터 제거하는 것으로 오류 회피
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i);
            if (child.TryGetComponent<ProductSlotUI>(out var slotUI))
            {
                //이벤트 해제
                slotUI.OnClicked -= HandleShopProductClicked;
                slotUI.OnClicked -= HandleWeaponInventoryProductClicked;
                slotUI.OnClicked -= HandleItemInventoryProductClicked;
                slotUI.OnPointerEntered -= HandleProductPointerEntered;
                slotUI.OnPointerExited -= HandleProductPointerExited;

                //오브젝트 풀에 반환
                pool.Release(slotUI);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }

    #region 이벤트 핸들러
    private void HandleShopProductClicked(IProduct product)
    {
        OnShopProductClicked?.Invoke(product);
    }

    private void HandleWeaponInventoryProductClicked(IProduct product)
    {
        OnWeaponInventoryProductClicked?.Invoke(product);
    }

    private void HandleItemInventoryProductClicked(IProduct product)
    {
        OnItemInventoryProductClicked?.Invoke(product);
    }

    private void HandleProductPointerEntered(IProduct product)
    {
        OnProductPointerEntered?.Invoke(product);
    }

    private void HandleProductPointerExited(IProduct product)
    {
        OnProductPointerExited?.Invoke(product);
    }
    #endregion

    //새로고침 코스트 설정
    public void SetRefreshCost(int cost)
    {
        _refreshCostText.text = cost.ToString();
    }
}