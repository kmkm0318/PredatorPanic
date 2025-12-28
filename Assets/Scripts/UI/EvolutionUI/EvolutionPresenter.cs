using System;
using UnityEngine;

/// <summary>
/// 진화 UI 프리젠터
/// </summary>
public class EvolutionPresenter : IPresenter, ITooltipProvider, ICancelable
{
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
        _evolutionUI.OnCloseButtonClicked += HandleOnCloseButtonClicked;
        _evolutionUI.OnEvolutionItemPointerEntered += HandleOnEvolutionItemPointerEntered;
        _evolutionUI.OnEvolutionItemPointerExited += HandleOnEvolutionItemPointerExited;
        _evolutionUI.OnEvolutionItemClicked += HandleOnEvolutionItemClicked;
    }

    private void UnregisterEvents()
    {
        _evolutionUI.OnCloseButtonClicked -= HandleOnCloseButtonClicked;
        _evolutionUI.OnEvolutionItemPointerEntered -= HandleOnEvolutionItemPointerEntered;
        _evolutionUI.OnEvolutionItemPointerExited -= HandleOnEvolutionItemPointerExited;
        _evolutionUI.OnEvolutionItemClicked -= HandleOnEvolutionItemClicked;
    }
    #endregion

    #region 이벤트 핸들러
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

        //다음 레벨 가격 계산
        int price = data.GetPriceForLevel(nextLevel);

        //현재 레벨로 희귀도 계산
        Rarity rarity = (Rarity)currentLevel;

        //레벨이 Rarity를 넘지 않도록
        if (rarity > Rarity.Legendary)
        {
            rarity = Rarity.Legendary;
        }

        //Rarity 색 가져오기
        Color targetColor = Color.gray;
        var rarityDict = DataManager.Instance.RarityDataList.RarityDataDict;
        if (rarityDict.TryGetValue(rarity, out var rarityData))
        {
            targetColor = rarityData.RarityColor;
        }

        //툴팁 요청 이벤트 호출
        OnTooltipRequested?.Invoke(new(
            data,
            $"{data.Name}({currentLevel}/{data.MaxLevel})",
            data.GetDescriptionByLevel(currentLevel),
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