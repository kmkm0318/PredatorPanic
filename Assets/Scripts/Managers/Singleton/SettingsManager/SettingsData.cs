using System;

/// <summary>
/// 설정 데이터 클래스
/// </summary>
[Serializable]
public class SettingsData
{
    //디스플레이 설정
    public int ResolutionIndex = 0;
    public bool IsFullScreen = true;
    public int RefreshRate = 60;

    //오디오 설정
    public int MasterVolume = 50;
    public int BGMVolume = 50;
    public int SFXVolume = 50;

    //입력 설정
    public float Sensitivity = 1.0f;

    //접근성 설정
    public bool EnableCameraShake = true;

    //기본 생성자
    public SettingsData() { }

    //복사 생성자
    public SettingsData(SettingsData other)
    {
        ResolutionIndex = other.ResolutionIndex;
        IsFullScreen = other.IsFullScreen;
        RefreshRate = other.RefreshRate;
        MasterVolume = other.MasterVolume;
        BGMVolume = other.BGMVolume;
        SFXVolume = other.SFXVolume;
        Sensitivity = other.Sensitivity;
        EnableCameraShake = other.EnableCameraShake;
    }
}