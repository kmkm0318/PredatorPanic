using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 레벨 업 보상 선택 UI 클래스
/// 레벨 업 시 플레이어가 받을 수 있는 보상을 선택하는 UI를 관리합니다.
/// </summary>
public class LevelUpRewardSelectUI : ShowHideUI
{
    [Header("UI Components")]
    [SerializeField] private PointerHandler _panelPointerHandler;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;

    [Header("Panel Scale Animation")]
    [SerializeField] private float _bigScale = 1.1f;
    [SerializeField] private float _smallScale = 0.9f;
    [SerializeField] private float _duration = 0.1f;
    [SerializeField] private Ease _easeType = Ease.OutBack;

    #region 데이터
    public LevelUpRewardData Data { get; private set; }
    #endregion

    #region 트윈
    private Tween _scaleTween;
    #endregion

    #region 이벤트
    public event Action<LevelUpRewardData> OnClicked;
    #endregion

    private void Awake()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        _panelPointerHandler.OnPointerEntered += OnPointerEntered;
        _panelPointerHandler.OnPointerExited += OnPointerExited;
        _panelPointerHandler.OnPointerDowned += OnPointerDowned;
        _panelPointerHandler.OnPointerUpped += OnPointerUpped;
        _panelPointerHandler.OnPointerClicked += OnPointerClicked;
    }

    private void UnregisterEvents()
    {
        _panelPointerHandler.OnPointerEntered -= OnPointerEntered;
        _panelPointerHandler.OnPointerExited -= OnPointerExited;
        _panelPointerHandler.OnPointerDowned -= OnPointerDowned;
        _panelPointerHandler.OnPointerUpped -= OnPointerUpped;
        _panelPointerHandler.OnPointerClicked -= OnPointerClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void OnPointerEntered()
    {
        SetPanelScale(_bigScale);
    }

    private void OnPointerExited()
    {
        SetPanelScale(1f);
    }

    private void OnPointerDowned()
    {
        SetPanelScale(_smallScale);
    }

    private void OnPointerUpped()
    {
        SetPanelScale(1f);
    }

    private void OnPointerClicked()
    {
        OnClicked?.Invoke(Data);
    }
    #endregion

    public void Init(LevelUpRewardData data)
    {
        Data = data;

        _iconImage.sprite = data.RewardIcon;
        _nameText.text = data.RewardName;
        _descriptionText.text = string.Join("\n", data.EffectDatas.ConvertAll(effect => effect.GetDescription()));
    }

    private void SetPanelScale(float scale)
    {
        _scaleTween?.Kill();

        _scaleTween = _panel.transform.DOScale(scale, _duration).SetEase(_easeType).SetUpdate(true);
    }
}