using UnityEngine;

/// <summary>
/// 입력 매니저 싱글톤 클래스
/// 플레이어 및 UI 입력 관리
/// </summary>
public class InputManager : Singleton<InputManager>
{
    public PlayerInputActions PlayerInputActions { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();
    }

    //플레이어 입력 활성화 및 커서 고정, 커서 숨기기
    public void EnablePlayerInput()
    {
        PlayerInputActions.Player.Enable();
        PlayerInputActions.UI.Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // UI 입력 활성화 및 커서 고정 해제, 커서 보이기
    public void EnableUIInput()
    {
        PlayerInputActions.Player.Disable();
        PlayerInputActions.UI.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableAllInput()
    {
        PlayerInputActions.Player.Disable();
        PlayerInputActions.UI.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}