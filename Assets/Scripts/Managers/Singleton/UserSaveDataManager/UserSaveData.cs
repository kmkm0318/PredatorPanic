using System;
using System.Collections.Generic;

/// <summary>
/// 사용자 저장 데이터 클래스
/// </summary>
[Serializable]
public class UserSaveData
{
    #region 상수
    private const string INITIAL_PLAYER_ID = "Player_Chicken";
    private const string INITIAL_WEAPON_ID = "Weapon_Gun_Glock";
    #endregion

    //DNA 재화
    public int DNA;

    //획득한 플레이어 리스트
    public List<string> AcquiredPlayers;

    //획득한 무기 리스트
    public List<string> AcquiredWeapons;

    //획득한 진화 정보 리스트
    public SerializableDictionary<string, int> AcquiredEvolutions;

    //마지막으로 선택한 플레이어 ID
    public string LastSelectedPlayerID;

    //마지막으로 선택한 무기 ID
    public string LastSelectedWeaponID;

    public UserSaveData()
    {
        DNA = 0;

        AcquiredPlayers = new()
        {
            //초기 플레이어는 항상 획득 상태로 시작
            INITIAL_PLAYER_ID
        };
        AcquiredWeapons = new()
        {
            //초기 무기는 항상 획득 상태로 시작
            INITIAL_WEAPON_ID
        };
        AcquiredEvolutions = new()
        {
            //초기 진화 정보는 빈 상태로 시작
        };

        LastSelectedPlayerID = INITIAL_PLAYER_ID;

        LastSelectedWeaponID = INITIAL_WEAPON_ID;
    }
}