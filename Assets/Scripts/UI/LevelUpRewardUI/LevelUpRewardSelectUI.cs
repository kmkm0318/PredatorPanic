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
    [SerializeField] private Outline _panelOutline;
    [SerializeField] private PointerHandler _panelPointerHandler;
    [SerializeField] private IconSlot _iconSlot;
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
        //패널 스케일 크게
        SetPanelScale(_bigScale);
    }

    private void OnPointerExited()
    {
        //패널 스케일 정상화
        SetPanelScale(1f);
    }

    private void OnPointerDowned()
    {
        //패널 스케일 작게
        SetPanelScale(_smallScale);
    }

    private void OnPointerUpped()
    {
        //패널 스케일 정상화
        SetPanelScale(1f);
    }

    private void OnPointerClicked()
    {
        //클릭 이벤트 호출
        OnClicked?.Invoke(Data);
    }
    #endregion

    public void Init(LevelUpRewardData data)
    {
        //데이터 할당
        Data = data;

        //희귀도에 따른 색상 설정
        var rarityDataList = DataManager.Instance.RarityDataList.GetRarityColor(data.Rarity);

        //아이콘 지정
        _iconSlot.SetIcon(data.RewardIcon);

        //배경 색상 설정
        _iconSlot.SetColor(rarityDataList);

        //외곽선 색상 설정
        _panelOutline.effectColor = rarityDataList;

        //이름 색 설정
        _nameText.color = rarityDataList;

        //이름 및 설명 지정
        _nameText.text = data.RewardName;
        _descriptionText.text = string.Join("\n", data.EffectDatas.ConvertAll(effect => effect.GetDescription()));
    }

    private void SetPanelScale(float scale)
    {
        _scaleTween?.Kill();

        _scaleTween = _panel.transform.DOScale(scale, _duration).SetEase(_easeType).SetUpdate(true);
    }
}