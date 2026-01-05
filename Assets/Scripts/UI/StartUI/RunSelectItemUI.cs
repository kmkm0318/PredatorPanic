using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RunSelectItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private PointerHandler _pointerHandler;
    [SerializeField] private IconSlot _iconSlot;
    [SerializeField] private Image _borderImage;
    [SerializeField] private GameObject _lockObj;
    [SerializeField] private DOTweenAnimation _clickAnimation;

    #region 데이터
    public RunData RunData { get; private set; }
    #endregion

    #region 이벤트
    public event Action<RunData> OnPointerEntered;
    public event Action<RunData> OnPointerExited;
    public event Action<RunData> OnPointerClicked;
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
        OnPointerEntered?.Invoke(RunData);
    }

    private void HandleOnPointerExited()
    {
        //이벤트 호출
        OnPointerExited?.Invoke(RunData);
    }

    private void HandlePointerClicked()
    {
        //이벤트 호출
        OnPointerClicked?.Invoke(RunData);

        //클릭 애니메이션 재생
        _clickAnimation.DORestart();
    }
    #endregion

    #region 초기화
    public void Init(RunData runData, bool isUnlocked, bool isSelected)
    {
        //데이터 설정
        RunData = runData;

        //아이콘 설정
        SetIcon(runData.Icon);

        //색상 설정
        UpdateColor(runData.Rarity);

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
        //아이콘 색 설정
        _iconSlot.SetColor(color);
    }

    public void UpdateUnlocked(bool isPurchased)
    {
        //잠금 오브젝트 활성화 설정
        _lockObj.SetActive(!isPurchased);
    }

    public void UpdateSelected(bool isSelected)
    {
        //테두리 활성화 설정
        _borderImage.gameObject.SetActive(isSelected);
    }
    #endregion
}
