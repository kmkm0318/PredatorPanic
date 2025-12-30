using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진화 UI 프리젠터
/// </summary>
public class EvolutionPresenter : IPresenter, ITooltipProvider, ICancelable
{
    #region 상수
    private const string GARY_COLOR_HEX = "#c0c0c0";
    #endregion

    #region 레퍼런스
    private EvolutionManager _evolutionManager;
    private EvolutionUI _evolutionUI;
    private ICancelableManager _cancelableManager;
    #endregion

    #region 이벤트
    public event Action OnClosed;
    public event Action<TooltipContext> OnTooltipRequested;
    public event Action<object> OnTooltipRequestCanceled;
    #endregion

    public EvolutionPresenter(EvolutionManager evolutionManager, EvolutionUI evolutionUI, ICancelableManager cancelableManager)
    {
        _evolutionManager = evolutionManager;
        _evolutionUI = evolutionUI;
        _cancelableManager = cancelableManager;
    }

    #region 초기화, 리셋
    public void Init()
    {
        RegisterEvents();

        InitUI();
    }

    private void InitUI()
    {
        //DNA 텍스트 초기화
        int dnaAmount = UserSaveDataManager.Instance.UserSaveData.DNA;
        _evolutionUI.UpdateDNAText(dnaAmount);

        //진화 아이템들 초기화
        var evolutionDataList = DataManager.Instance.EvolutionDataList.EvolutionDatas;
        _evolutionUI.UpdateEvolutionItems(evolutionDataList);
    }

    public void Reset()
    {
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        UserSaveDataManager.Instance.OnDNAChanged += HandleOnDNAChanged;

        _evolutionUI.OnCloseButtonClicked += HandleOnCloseButtonClicked;
        _evolutionUI.OnEvolutionItemPointerEntered += HandleOnEvolutionItemPointerEntered;
        _evolutionUI.OnEvolutionItemPointerExited += HandleOnEvolutionItemPointerExited;
        _evolutionUI.OnEvolutionItemClicked += HandleOnEvolutionItemClicked;
    }

    private void UnregisterEvents()
    {
        UserSaveDataManager.Instance.OnDNAChanged += HandleOnDNAChanged;

        _evolutionUI.OnCloseButtonClicked -= HandleOnCloseButtonClicked;
        _evolutionUI.OnEvolutionItemPointerEntered -= HandleOnEvolutionItemPointerEntered;
        _evolutionUI.OnEvolutionItemPointerExited -= HandleOnEvolutionItemPointerExited;
        _evolutionUI.OnEvolutionItemClicked -= HandleOnEvolutionItemClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnDNAChanged(int dnaAmount)
    {
        _evolutionUI.UpdateDNAText(dnaAmount);
    }

    private void HandleOnCloseButtonClicked()
    {
        //UI 숨기기
        Hide();
    }

    private void HandleOnEvolutionItemPointerEntered(EvolutionData data)
    {
        //현재 레벨 가져오기
        int currentLevel = UserSaveDataManager.Instance.GetEvolutionLevel(data.ID);

        //다음 레벨 계산
        int nextLevel = currentLevel + 1;

        //맥스 레벨인지?
        bool isMaxLevel = currentLevel >= data.MaxLevel;

        //다음 레벨 가격 계산
        int price = data.GetPriceForLevel(nextLevel);

        //설명 생성
        List<string> descriptions = new();

        //현재 레벨에 0이 아니면
        if (currentLevel != 0)
        {
            //현재 레벨 설명 추가
            descriptions.Add($"({currentLevel}/{data.MaxLevel})\n{data.GetDescriptionByLevel(currentLevel)}");
        }

        //맥스 레벨이 아니면
        if (!isMaxLevel)
        {
            //다음 레벨 설명 회색으로 추가
            descriptions.Add($"<color={GARY_COLOR_HEX}>({nextLevel}/{data.MaxLevel}):\n{data.GetDescriptionByLevel(nextLevel)}</color>");
        }

        //전체 설명 합치기
        string description = string.Join("\n\n", descriptions);

        //Rarity 색 가져오기
        Color targetColor = DataManager.Instance.RarityDataList.GetRarityColor(currentLevel - 1);

        //툴팁 요청 이벤트 호출
        OnTooltipRequested?.Invoke(new(
            data,
            $"{data.Name}({currentLevel}/{data.MaxLevel})",
            description,
            targetColor,
            data.Icon,
            price
        ));
    }

    private void HandleOnEvolutionItemPointerExited(EvolutionData data)
    {
        //툴팁 요청 취소 이벤트 호출
        OnTooltipRequestCanceled?.Invoke(data);
    }

    private void HandleOnEvolutionItemClicked(EvolutionData data)
    {
        //진화 시도
        if (_evolutionManager.TryUpgradeEvolution(data))
        {
            //성공 시 Tooltip 숨기기
            OnTooltipRequestCanceled?.Invoke(data);
        }
        else
        {
            //실패 시 아무것도 하지 않음
        }
    }
    #endregion

    #region Show, Hide
    public void Show()
    {
        //UI 표시
        _evolutionUI.Show(0f);

        //취소 가능한 항목으로 등록
        _cancelableManager.PushCancelable(this);
    }

    public void Hide()
    {
        //UI 숨기기
        _evolutionUI.Hide(0f);

        //취소 가능한 항목에서 제거
        _cancelableManager.PopCancelable(this);

        //툴팁 요청 취소
        OnTooltipRequestCanceled?.Invoke(null);

        //닫힘 이벤트 호출
        OnClosed?.Invoke();
    }
    #endregion

    public void Cancel()
    {
        //닫기 버튼을 누른 것과 같이 처리
        HandleOnCloseButtonClicked();
    }
}