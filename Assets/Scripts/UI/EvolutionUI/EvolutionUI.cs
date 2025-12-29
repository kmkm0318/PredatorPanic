using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// 진화 UI 클래스
/// </summary>
public class EvolutionUI : ShowHideUI
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _dnaText;
    [SerializeField] private EvolutionItemUI _evolutionItemUIPrefab;
    [SerializeField] private Transform _evolutionItemUIParent;
    [SerializeField] private Button _closeButton;

    #region 오브젝트 풀
    private ObjectPool<EvolutionItemUI> _evolutionItemUIPool;
    #endregion

    #region 이벤트
    public event Action OnCloseButtonClicked;
    public event Action<EvolutionData> OnEvolutionItemClicked;
    public event Action<EvolutionData> OnEvolutionItemPointerEntered;
    public event Action<EvolutionData> OnEvolutionItemPointerExited;
    #endregion

    private void Awake()
    {
        //닫기 버튼 이벤트 구독
        _closeButton.onClick.AddListener(() =>
        {
            OnCloseButtonClicked?.Invoke();
        });
    }

    #region 오브젝트 풀링
    private void InitPool()
    {
        _evolutionItemUIPool = new(
            () => Instantiate(_evolutionItemUIPrefab, _evolutionItemUIParent),
            item =>
            {
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
            },
            item => item.gameObject.SetActive(false),
            item => Destroy(item.gameObject)
        );
    }
    #endregion

    #region UI 업데이트
    public void UpdateDNAText(int dnaAmount)
    {
        _dnaText.text = dnaAmount.ToString();
    }

    public void UpdateEvolutionItems(List<EvolutionData> evolutionDatas)
    {
        //풀 초기화
        if (_evolutionItemUIPool == null) InitPool();

        //기존 아이템 UI 반환
        while (_evolutionItemUIParent.childCount > 0)
        {
            //자식 가져오기
            var child = _evolutionItemUIParent.GetChild(0);

            if (child.TryGetComponent<EvolutionItemUI>(out var itemUI))
            {
                //이벤트 해제
                itemUI.OnPointerEntered -= HandleOnPointerEntered;
                itemUI.OnPointerExited -= HandleOnPointerExited;
                itemUI.OnPointerClicked -= HandleOnPointerClicked;

                //오브젝트 풀에 반환
                _evolutionItemUIPool.Release(itemUI);
            }
            else
            {
                //존재하지 않으면 파괴
                Destroy(child.gameObject);
            }
        }

        //진화 아이템 UI 생성 및 초기화
        foreach (var evolutionData in evolutionDatas)
        {
            //오브젝트 풀에서 가져오기
            var itemUI = _evolutionItemUIPool.Get();

            //초기화
            itemUI.Init(evolutionData);

            //이벤트 구독
            itemUI.OnPointerEntered += HandleOnPointerEntered;
            itemUI.OnPointerExited += HandleOnPointerExited;
            itemUI.OnPointerClicked += HandleOnPointerClicked;
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnPointerEntered(EvolutionData data)
    {
        OnEvolutionItemPointerEntered?.Invoke(data);
    }

    private void HandleOnPointerExited(EvolutionData data)
    {
        OnEvolutionItemPointerExited?.Invoke(data);
    }

    private void HandleOnPointerClicked(EvolutionData data)
    {
        OnEvolutionItemClicked?.Invoke(data);
    }
    #endregion
}
