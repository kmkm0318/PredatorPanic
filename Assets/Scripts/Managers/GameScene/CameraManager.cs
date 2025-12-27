using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 카메라 매니저 클래스
/// </summary>
public class CameraManager : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private CinemachineInputAxisController _cinemachineInputAxisController;

    private void Start()
    {
        //이벤트 등록
        RegisterEvents();

        //초기화
        InitSettings();
    }

    private void InitSettings()
    {
        //설정 데이터로 초기화
        HandleOnSettingsChanged(SettingsManager.Instance.CurrentData);

        //현재 입력 모드로 초기화
        HandleOnInputModeChanged(InputManager.Instance.CurrentInputMode);
    }

    private void OnDestroy()
    {
        //이벤트 해제
        UnregisterEvents();
    }

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        SettingsManager.Instance.OnSettingsChanged += HandleOnSettingsChanged;
        InputManager.Instance.OnInputModeChanged += HandleOnInputModeChanged;
    }

    private void UnregisterEvents()
    {
        SettingsManager.Instance.OnSettingsChanged -= HandleOnSettingsChanged;
        InputManager.Instance.OnInputModeChanged -= HandleOnInputModeChanged;
    }
    #endregion


    #region 이벤트 핸들러
    private void HandleOnSettingsChanged(SettingsData data)
    {
        //컨트롤러 가져오기
        var controllers = _cinemachineInputAxisController.Controllers;

        if (controllers.Count > 0)
        {
            //X축 민감도 설정
            _cinemachineInputAxisController.Controllers[0].Input.Gain = data.Sensitivity;
        }

        if (controllers.Count > 1)
        {
            //Y축 민감도 설정
            _cinemachineInputAxisController.Controllers[1].Input.Gain = data.Sensitivity;
        }
    }

    private void HandleOnInputModeChanged(InputMode mode)
    {
        //플레이어 모드일 때만 카메라 입력 활성화
        _cinemachineInputAxisController.enabled = mode == InputMode.Player;
    }
    #endregion
}