using System;
using System.Collections.Generic;

/// <summary>
/// 글로벌 게임 매니저
/// 메인 메뉴 씬에서 선택한 플레이어, 무기 데이터를 게임 씬으로 전달
/// </summary>
public class GlobalGameManager : Singleton<GlobalGameManager>
{
    #region 선택된 데이터
    public PlayerData SelectedPlayerData { get; private set; }
    public WeaponData SelectedWeaponData { get; private set; }
    public Dictionary<EvolutionData, int> AppliedEvolutions { get; private set; } = new();
    #endregion

    #region 이벤트
    public event Action<PlayerData> OnSelectedPlayerDataChanged;
    public event Action<WeaponData> OnSelectedWeaponDataChanged;
    #endregion

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //매니저 인스턴스 가져오기
        var userSaveDataManager = UserSaveDataManager.Instance;
        var dataManager = DataManager.Instance;

        //마지막으로 선택된 플레이어, 무기 ID 가져오기
        var lastSelectedPlayerID = userSaveDataManager.UserSaveData.LastSelectedPlayerID;
        var lastSelectedWeaponID = userSaveDataManager.UserSaveData.LastSelectedWeaponID;

        //데이터 매니저에서 해당 ID의 플레이어, 무기 데이터 가져오기
        var playerData = dataManager.PlayerDataList.GetData(lastSelectedPlayerID);
        var weaponData = dataManager.WeaponDataList.GetData(lastSelectedWeaponID);

        //선택된 플레이어, 무기 데이터 설정
        SetSelectedPlayerData(playerData);
        SetSelectedWeaponData(weaponData);
    }

    /// <summary>
    /// 선택된 플레이어 데이터 설정
    /// </summary>
    public void SetSelectedPlayerData(PlayerData playerData)
    {
        SelectedPlayerData = playerData;
        OnSelectedPlayerDataChanged?.Invoke(playerData);
    }

    /// <summary>
    /// 선택된 무기 데이터 설정
    /// </summary>
    public void SetSelectedWeaponData(WeaponData weaponData)
    {
        SelectedWeaponData = weaponData;
        OnSelectedWeaponDataChanged?.Invoke(weaponData);
    }

    /// <summary>
    /// 적용된 진화 정보 설정
    /// </summary>
    public void InitAppliedEvolutions()
    {
        //매니저 인스턴스 가져오기
        var userSaveDataManager = UserSaveDataManager.Instance;
        var dataManager = DataManager.Instance;

        //사용자 저장 데이터에서 획득한 진화 정보 가져오기
        var acquiredEvolutions = userSaveDataManager.UserSaveData.AcquiredEvolutions.Dictionary;

        //데이터 매니저에서 진화 데이터 리스트 가져오기
        var evolutionDataList = dataManager.EvolutionDataList;

        //적용된 진화 정보 초기화
        AppliedEvolutions.Clear();

        //획득한 진화 정보를 기반으로 적용된 진화 정보 설정
        foreach (var kvp in acquiredEvolutions)
        {
            var evolutionData = evolutionDataList.GetData(kvp.Key);
            if (evolutionData != null)
            {
                AppliedEvolutions[evolutionData] = kvp.Value;
            }
        }
    }
}