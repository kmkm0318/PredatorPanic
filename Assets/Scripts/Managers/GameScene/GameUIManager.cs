using UnityEngine;

/// <summary>
/// 게임 UI 매니저
/// 게임 씬의 UI 요소들을 관리
/// </summary>
public class GameUIManager : MonoBehaviour
{
    [SerializeField] private PlayerUI _playerUI;

    public void Init(Player player)
    {
        _playerUI.Init(player);
    }
}