using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 클리어 UI
/// </summary>
public class GameResultUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private IrisFade _background;
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _dnaText;
    [SerializeField] private Ease _dnaEaseType = Ease.OutCubic;
    [SerializeField] private Button _mainMenuButton;

    #region 이벤트
    public event Action OnMainMenuButtonClicked;
    #endregion

    private void Awake()
    {
        // 메인 메뉴 버튼 클릭 이벤트 등록
        _mainMenuButton.onClick.AddListener(() => OnMainMenuButtonClicked?.Invoke());
    }

    #region UI 설정
    public void SetTitle(string title)
    {
        // 제목 설정
        _titleText.text = title;
    }

    public void SetDNAText(int dnaAmount)
    {
        // DNA 텍스트 설정
        _dnaText.text = $"획득한 DNA: {dnaAmount}";
    }

    public void UpdateDNAText(int dna, float duration = 1f, Action onComplete = null)
    {
        // DNA 텍스트 업데이트 애니메이션
        int currentDna = 0;
        DOTween.To(() => currentDna, x => currentDna = x, dna, duration)
            .OnUpdate(() => SetDNAText(currentDna))
            .SetEase(_dnaEaseType)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void ShowMainMenuButton(bool show)
    {
        // 버튼 활성화
        _mainMenuButton.gameObject.SetActive(show);
    }

    public void PlayMainMenuPunchAnimation(float duration = 0.5f, Action onComplete = null)
    {
        // 펀치 애니메이션
        _mainMenuButton.transform.DOPunchRotation(new Vector3(0, 0, 10f), duration).OnComplete(() => onComplete?.Invoke());
    }
    #endregion

    #region Show, Hide
    public void Show(float irisDuration = 1f, float panelDuration = 0.5f, Action onComplete = null)
    {
        //패널 알파 초기화
        _panel.alpha = 0f;

        //게임 오브젝트 활성화
        gameObject.SetActive(true);

        //아이리스 페이드 아웃
        _background.IrisOut(irisDuration, () =>
        {
            //패널 페이드 인
            _panel.DOFade(1f, panelDuration).OnComplete(() => onComplete?.Invoke());
        });
    }

    public void Hide(float panelDuration = 0.5f, float irisDuration = 1f, Action onComplete = null)
    {
        //패널 페이드 아웃
        _panel.DOFade(0f, panelDuration).OnComplete(() =>
        {
            //아이리스 페이드 인
            _background.IrisIn(irisDuration, () =>
            {
                //게임 오브젝트 비활성화
                gameObject.SetActive(false);

                //완료 콜백 호출
                onComplete?.Invoke();
            });
        });
    }
    #endregion
}