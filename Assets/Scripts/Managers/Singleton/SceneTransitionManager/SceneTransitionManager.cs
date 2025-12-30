using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 매니저 클래스
/// </summary>
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    #region 상수
    public const string MAIN_MENU_SCENE_NAME = "MainMenuScene";
    public const string GAME_SCENE_NAME = "GameScene";
    private const float SCENE_SHOW_DELAY = 0.5f;
    #endregion

    [Header("UI Elements")]
    [SerializeField] private SceneTransitionUI _sceneTransitionUI;

    #region 변수
    private bool _isTransitioning = false;
    #endregion

    #region 이벤트
    public event Action OnSceneTransitionStarted;
    public event Action OnSceneTransitionCompleted;
    #endregion

    public void ChangeScene(string sceneName, float duration = 1f)
    {
        // 중복 호출 방지
        if (_isTransitioning) return;

        // 전환 상태 설정
        _isTransitioning = true;

        // 코루틴 시작
        StartCoroutine(ChangeSceneCoroutine(sceneName, duration));
    }

    private IEnumerator ChangeSceneCoroutine(string sceneName, float duration)
    {
        // 씬 전환 시작 이벤트 호출
        OnSceneTransitionStarted?.Invoke();

        //시간 절반 계산
        float halfDuration = duration / 2f;

        //씬 전환 UI 보여주기
        bool isShown = false;
        _sceneTransitionUI.Show(halfDuration, () => isShown = true);
        yield return new WaitUntil(() => isShown);

        //씬 로드
        SceneManager.LoadScene(sceneName);

        //대기 시간
        yield return new WaitForSeconds(SCENE_SHOW_DELAY);

        //씬 전환 UI 숨기기
        bool isHidden = false;
        _sceneTransitionUI.Hide(halfDuration, () => isHidden = true);
        yield return new WaitUntil(() => isHidden);

        //씬 전환 완료 이벤트 호출
        OnSceneTransitionCompleted?.Invoke();

        //전환 상태 초기화
        _isTransitioning = false;
    }
}