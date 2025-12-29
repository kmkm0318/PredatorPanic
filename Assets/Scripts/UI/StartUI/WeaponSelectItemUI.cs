using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 무기 선택 아이템 UI
/// 무기를 구입하거나 선택할 수 있는 UI
/// </summary>
public class WeaponSelectItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private PointerHandler _pointerHandler;
    [SerializeField] private IconSlot _iconSlot;
    [SerializeField] private Image _borderImage;
    [SerializeField] private GameObject _lockObj;
    [SerializeField] private DOTweenAnimation _clickAnimation;

    #region 데이터
    public WeaponData WeaponData { get; private set; }
    #endregion

    #region 이벤트
    public event Action<WeaponData> OnPointerEntered;
    public event Action<WeaponData> OnPointerExited;
    public event Action<WeaponData> OnPointerClicked;
    #endregion

    private void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        _pointerHandler.OnPointerEntered += HandleOnPointerEntered;
        _pointerHandler.OnPointerExited += HandleOnPointerExited;
        _pointerHandler.OnPointerClicked += HandlePointerClicked;
    }

    #region 버튼 이벤트 핸들러
    private void HandleOnPointerEntered()
    {
        //이벤트 호출
        OnPointerEntered?.Invoke(WeaponData);
    }

    private void HandleOnPointerExited()
    {
        //이벤트 호출
        OnPointerExited?.Invoke(WeaponData);
    }

    private void HandlePointerClicked()
    {
        //이벤트 호출
        OnPointerClicked?.Invoke(WeaponData);

        //클릭 애니메이션 재생
        _clickAnimation.DORestart();
    }
    #endregion

    #region 초기화
    public void Init(WeaponData weaponData, bool isUnlocked, bool isSelected)
    {
        //데이터 설정
        WeaponData = weaponData;

        //아이콘 설정
        SetIcon(weaponData.Icon);

        //색상 설정
        UpdateColor(weaponData.Rarity);

        //잠금 상태 설정
        UpdateUnlocked(isUnlocked);

        //선택 상태 설정
        UpdateSelected(isSelected);
    }
    #endregion

    #region UI 업데이트
    public void SetIcon(Sprite sprite)
    {
        _iconSlot.SetIcon(sprite);
    }

    public void UpdateColor(Rarity rarity)
    {
        //색 가져오기
        Color color = DataManager.Instance.RarityDataList.GetRarityColor(rarity);

        //색 변경
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        _iconSlot.SetColor(color);
    }

    public void UpdateUnlocked(bool isUnlocked)
    {
        //잠금 오브젝트 활성화 설정
        _lockObj.SetActive(!isUnlocked);
    }

    public void UpdateSelected(bool isSelected)
    {
        //테두리 활성화 설정
        _borderImage.gameObject.SetActive(isSelected);
    }
    #endregion
}