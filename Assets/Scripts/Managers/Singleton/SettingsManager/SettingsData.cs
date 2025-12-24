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
    public float MasterVolume = 0.5f;
    public float BGMVolume = 0.5f;
    public float SFXVolume = 0.5f;
}