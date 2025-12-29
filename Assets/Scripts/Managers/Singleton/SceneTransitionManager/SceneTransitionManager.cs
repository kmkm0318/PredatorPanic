using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 매니저 클래스
/// </summary>
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    #region 상수
    public const string MAIN_MENU_SCENE_NAME = "MainMenuScene";
    public const string GAMEPLAY_NAME = "GameScene";
    #endregion

    [Header("UI Elements")]
    [SerializeField] private SceneTransitionUI _sceneTransitionUI;

    #region 이벤트
    public event Action OnSceneTransitionStarted;
    public event Action OnSceneTransitionCompleted;
    #endregion

    public void ChangeScene(string sceneName, float duration = 1f)
    {
        // 씬 전환 시작 이벤트 호출
        OnSceneTransitionStarted?.Invoke();

        float halfDuration = duration / 2f;

        // 씬 전환 UI 보여주기
        _sceneTransitionUI.Show(halfDuration, () =>
        {
            // 씬 로드
            SceneManager.LoadScene(sceneName);

            // 씬 전환 UI 숨기기
            _sceneTransitionUI.Hide(halfDuration, () =>
            {
                // 씬 전환 완료 이벤트 호출
                OnSceneTransitionCompleted?.Invoke();
            });
        });
    }
}