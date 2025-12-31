using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 진화 아이템 UI 클래스
/// </summary>
public class EvolutionItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private IconSlot _iconSlot;
    [SerializeField] private PointerHandler _pointerHandler;
    [SerializeField] private DOTweenAnimation _clickAnimation;

    #region 데이터
    private EvolutionData _evolutionData;
    #endregion

    #region 이벤트
    public event Action<EvolutionData> OnPointerEntered;
    public event Action<EvolutionData> OnPointerExited;
    public event Action<EvolutionData> OnPointerClicked;
    #endregion

    private void Awake()
    {
        //포인터 이벤트 구독
        RegisterPointerEvents();
    }

    #region 버튼 이벤트
    private void RegisterPointerEvents()
    {
        _pointerHandler.OnPointerEntered += HandleOnPointerEntered;
        _pointerHandler.OnPointerExited += HandleOnPointerExited;
        _pointerHandler.OnPointerClicked += HandleOnPointerClicked;
    }

    private void HandleOnPointerEntered()
    {
        //이벤트 호출
        OnPointerEntered?.Invoke(_evolutionData);
    }

    private void HandleOnPointerExited()
    {
        //이벤트 호출
        OnPointerExited?.Invoke(_evolutionData);
    }

    private void HandleOnPointerClicked()
    {
        //클릭 애니메이션 재생
        _clickAnimation.DORestart();

        //이벤트 호출
        OnPointerClicked?.Invoke(_evolutionData);

        //색 업데이트
        UpdateBackgroundColor();
    }
    #endregion

    #region 초기화
    public void Init(EvolutionData evolutionData)
    {
        //데이터 설정
        _evolutionData = evolutionData;

        //아이콘 설정
        _iconSlot.SetIcon(_evolutionData.Icon);

        //색 설정
        UpdateBackgroundColor();
    }
    #endregion

    #region UI 업데이트
    public void UpdateBackgroundColor()
    {
        //데이터 없으면 패스
        if (_evolutionData == null) return;

        //레벨 가져오기
        var currentLevel = UserSaveDataManager.Instance.GetEvolutionLevel(_evolutionData.ID);

        //희귀도 색상 가져오기
        var color = DataManager.Instance.RarityDataList.GetRarityColor(currentLevel - 1);

        //색 적용
        _iconSlot.SetColor(color);
    }
    #endregion
}