using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설정 매니저 싱글톤 클래스
/// </summary>
public class SettingsManager : Singleton<SettingsManager>
{
    #region 변수
    private string _savePath;
    public SettingsData CurrentData { get; private set; }
    public List<(int, int)> ResolutionOptions { get; private set; } = new();
    #endregion

    protected override void Awake()
    {
        base.Awake();

        //세이브 경로 초기화
        InitSavePath();

        //해상도 옵션 초기화
        InitResolutions();

        //설정 불러오기
        LoadSettings();
    }

    private void InitSavePath()
    {
        //Persistent Data Path에 세이브 파일 경로 설정
        _savePath = Application.persistentDataPath + "/settings.json";
    }

    private void InitResolutions()
    {
        //해상도 옵션 생성
        ResolutionOptions = new();

        //중복 제거 후 해상도 옵션 추가
        foreach (var res in Screen.resolutions)
        {
            var option = (res.width, res.height);

            if (ResolutionOptions.Contains(option)) continue;

            ResolutionOptions.Add(option);
        }
    }

    private void Start()
    {
        //설정 적용
        ApplySettings();
    }

    #region 세이브, 로드
    public void SaveSettings()
    {
        //json 형태로 변경
        var json = JsonUtility.ToJson(CurrentData);

        //파일에 저장
        System.IO.File.WriteAllText(_savePath, json);
    }

    private void LoadSettings()
    {
        if (System.IO.File.Exists(_savePath))
        {
            try
            {
                //파일에서 json 불러오기
                var json = System.IO.File.ReadAllText(_savePath);

                //SettingsData 형태로 변환
                CurrentData = JsonUtility.FromJson<SettingsData>(json);
            }
            catch
            {
                //오류 발생 시 기본 설정 생성
                CurrentData = new SettingsData();
            }
        }
        else
        {
            //파일이 존재하지 않으면 기본 설정 생성
            CurrentData = new SettingsData();
        }
    }
    #endregion

    #region 설정 적용
    public void ApplySettings()
    {
        ApplyDisplaySettings();
        ApplyAudioSettings();
    }

    private void ApplyDisplaySettings()
    {
        //범위 밖에 있는 해상도 인덱스 보정
        if (CurrentData.ResolutionIndex < 0)
        {
            CurrentData.ResolutionIndex = 0;
        }
        else if (CurrentData.ResolutionIndex >= ResolutionOptions.Count)
        {
            CurrentData.ResolutionIndex = ResolutionOptions.Count - 1;
        }

        var resolution = ResolutionOptions[CurrentData.ResolutionIndex];

        Screen.SetResolution(resolution.Item1, resolution.Item2, CurrentData.IsFullScreen);
    }

    private void ApplyAudioSettings()
    {
        AudioManager.Instance.SetMasterVolume(CurrentData.MasterVolume);
        AudioManager.Instance.SetBGMVolume(CurrentData.BGMVolume);
        AudioManager.Instance.SetSFXVolume(CurrentData.SFXVolume);
    }
    #endregion
}
