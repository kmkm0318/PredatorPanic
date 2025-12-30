using System;

/// <summary>
/// 게임 결과 프리젠터
/// </summary>
public class GameResultPresenter : IPresenter
{
    #region 레퍼런스
    private GameResultUI _gameResultUI;
    #endregion

    public GameResultPresenter(GameResultUI gameResultUI)
    {
        _gameResultUI = gameResultUI;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        RegisterEvents();
    }

    public void Reset()
    {
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        _gameResultUI.OnMainMenuButtonClicked += HandleOnMainMenuButtonClicked;
    }

    private void UnregisterEvents()
    {
        _gameResultUI.OnMainMenuButtonClicked -= HandleOnMainMenuButtonClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnMainMenuButtonClicked()
    {
        //메인 메뉴 씬으로 전환
        SceneTransitionManager.Instance.ChangeScene(SceneTransitionManager.MAIN_MENU_SCENE_NAME);
    }
    #endregion

    public void ShowGameResult(string title, int dna, Action onComplete = null)
    {
        //DNA 초기화
        _gameResultUI.SetDNAText(0);

        //메인 메뉴 버튼 숨기기
        _gameResultUI.ShowMainMenuButton(false);

        //타이틀 설정
        _gameResultUI.SetTitle(title);

        //게임 결과 UI 표시
        _gameResultUI.Show(onComplete: () =>
        {
            //DNA 애니메이션 표시
            _gameResultUI.UpdateDNAText(dna: dna, onComplete: () =>
            {
                //메인 메뉴 버튼 표시
                _gameResultUI.ShowMainMenuButton(true);

                //메인 메뉴 버튼 펀치 애니메이션 재생
                _gameResultUI.PlayMainMenuPunchAnimation();

                //완료 콜백 호출
                onComplete?.Invoke();
            });
        });
    }
};