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
        //현재 설정 데이터 가져오기
        var settingsData = SettingsManager.Instance.CurrentData;

        //민감도 설정 적용
        UpdateSensitivity(settingsData.Sensitivity);
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
    }

    private void UnregisterEvents()
    {
        SettingsManager.Instance.OnSettingsChanged -= HandleOnSettingsChanged;
    }
    #endregion


    #region 이벤트 핸들러
    private void HandleOnSettingsChanged(SettingsData data)
    {
        UpdateSensitivity(data.Sensitivity);
    }

    //민감도 설정 적용
    private void UpdateSensitivity(float sensitivity)
    {
        //컨트롤러 가져오기
        var controllers = _cinemachineInputAxisController.Controllers;

        if (controllers.Count > 0)
        {
            //X축 민감도 설정
            _cinemachineInputAxisController.Controllers[0].Input.Gain = sensitivity;
        }

        if (controllers.Count > 1)
        {
            //Y축 민감도 설정
            _cinemachineInputAxisController.Controllers[1].Input.Gain = sensitivity;
        }
    }
    #endregion
}