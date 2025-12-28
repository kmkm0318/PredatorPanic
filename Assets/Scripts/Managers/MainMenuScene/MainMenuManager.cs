using UnityEngine;

/// <summary>
/// 메인 메뉴의 전반적인 관리를 담당하는 클래스
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    #region 레퍼런스
    [SerializeField] private MainMenuUIManager _mainMenuUIManager;
    [SerializeField] private EvolutionManager _evolutionManager;

    public MainMenuUIManager MainMenuUIManager => _mainMenuUIManager;
    public EvolutionManager EvolutionManager => _evolutionManager;
    #endregion

    private void Start()
    {
        InitManagers();
    }

    private void InitManagers()
    {
        _mainMenuUIManager.Init(this);
        _evolutionManager.Init(this);
    }
}
