using UnityEngine;

/// <summary>
/// 로딩 씬 매니저 클래스
/// 로딩 씬에서 로딩이 끝나면 메인 메뉴 씬으로 전환하는 역할
/// </summary>
public class LoadingManager : MonoBehaviour
{
    private void Start()
    {
        //메인 메뉴 씬으로 전환
        SceneTransitionManager.Instance.ChangeScene(SceneTransitionManager.MAIN_MENU_SCENE_NAME);
    }
}