using UnityEngine;

/// <summary>
/// 사용자 저장 데이터 매니저 싱글톤 클래스
/// 게임 내에서 영구적으로 저장되는 사용자 데이터를 관리합니다.
/// </summary>
public class UserSaveDataManager : Singleton<UserSaveDataManager>
{
    #region 변수
    private string _savePath;
    public UserSaveData UserSaveData { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    #region 초기화
    private void Init()
    {
        //세이브 경로 초기화
        InitSavePath();

        //초기 사용자 저장 데이터 로드
        LoadUserSaveData();
    }

    private void InitSavePath()
    {
        _savePath = Application.persistentDataPath + "/userSaveData.json";
    }
    #endregion

    #region 세이브, 로드
    public void SaveUserSaveData()
    {
        //Json 으로 변경
        string json = JsonUtility.ToJson(UserSaveData);

        //파일로 저장
        System.IO.File.WriteAllText(_savePath, json);
    }
    private void LoadUserSaveData()
    {
        if (System.IO.File.Exists(_savePath))
        {
            try
            {
                //파일에서 json 읽기
                string json = System.IO.File.ReadAllText(_savePath);

                //Json을 객체로 변환
                UserSaveData = JsonUtility.FromJson<UserSaveData>(json);
            }
            catch
            {
                //에러 발생 시 기본값으로 초기화
                UserSaveData = new();
            }
        }
        else
        {
            //파일이 없으면 기본값으로 초기화
            UserSaveData = new();
        }
    }
    #endregion
}