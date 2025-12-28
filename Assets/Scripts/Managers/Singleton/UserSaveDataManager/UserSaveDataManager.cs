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
        //진화 리스트 업데이트
        UserSaveData.AcquiredEvolutions.UpdateList();

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

    #region DNA
    public void AddDNA(int amount)
    {
        //DNA 추가
        UserSaveData.DNA += amount;

        //최대, 최소 클램핑
        UserSaveData.DNA = Mathf.Clamp(UserSaveData.DNA, 0, int.MaxValue);
    }

    public bool TrySpendDNA(int amount)
    {
        //DNA가 충분한지 확인
        if (UserSaveData.DNA >= amount)
        {
            //DNA 차감
            UserSaveData.DNA -= amount;
            return true;
        }
        return false;
    }
    #endregion

    #region 플레이어
    public void AddPlayerData(string playerID)
    {
        //중복 방지
        if (UserSaveData.AcquiredPlayers.Contains(playerID)) return;

        //플레이어 ID 추가
        UserSaveData.AcquiredPlayers.Add(playerID);
    }
    #endregion

    #region 무기
    public void AddWeaponData(string weaponID)
    {
        //중복 방지
        if (UserSaveData.AcquiredWeapons.Contains(weaponID)) return;

        //무기 ID 추가
        UserSaveData.AcquiredWeapons.Add(weaponID);
    }
    #endregion

    #region 진화
    public int GetEvolutionLevel(string evolutionID)
    {
        //진화 ID가 존재하는지 확인
        if (UserSaveData.AcquiredEvolutions.Dictionary.TryGetValue(evolutionID, out int level))
        {
            return level;
        }

        //존재하지 않으면 레벨 0 반환
        return 0;
    }

    public void UpdateEvolutionLevel(string evolutionID, int level)
    {
        //진화 레벨 증가
        UserSaveData.AcquiredEvolutions.Dictionary[evolutionID] = level;
    }
    #endregion
}