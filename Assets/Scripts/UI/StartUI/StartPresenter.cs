using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시작 프레젠터
/// 메인 메뉴의 시작 UI를 제어
/// </summary>
public class StartPresenter : IPresenter, ICancelable, ITooltipProvider
{
    #region 레퍼런스
    private StartUI _startUI;
    private ICancelableManager _cancelableManager;
    #endregion

    #region 이벤트
    public event Action OnClosed;
    public event Action<TooltipContext> OnTooltipRequested;
    public event Action<object> OnTooltipRequestCanceled;
    #endregion

    public StartPresenter(StartUI startUI, ICancelableManager cancelableManager)
    {
        _startUI = startUI;
        _cancelableManager = cancelableManager;
    }

    #region 초기화 및 리셋
    public void Init()
    {
        RegisterEvents();

        InitUI();
    }

    private void InitUI()
    {
        //DNA 텍스트 초기화
        HandleDNAChanged(UserSaveDataManager.Instance.UserSaveData.DNA);

        //선택된 플레이어 초기화
        HandleOnSelectedPlayerDataChanged(GlobalGameManager.Instance.SelectedPlayerData);

        //선택된 무기 초기화
        HandleOnSelectedWeaponDataChanged(GlobalGameManager.Instance.SelectedWeaponData);

        //플레이어 선택 아이템 UI 초기화
        List<PlayerSelectItemContext> playerSelectItemContexts = new();
        foreach (var playerData in DataManager.Instance.PlayerDataList.PlayerDatas)
        {
            bool isUnlocked = UserSaveDataManager.Instance.IsPlayerUnlocked(playerData.ID);
            bool isSelected = GlobalGameManager.Instance.SelectedPlayerData == playerData;

            playerSelectItemContexts.Add(new(playerData, isUnlocked, isSelected));
        }

        _startUI.SetPlayerSelectItems(playerSelectItemContexts);

        //무기 선택 아이템 UI 초기화
        List<WeaponSelectItemContext> weaponSelectItemContexts = new();

        foreach (var weaponData in DataManager.Instance.WeaponDataList.WeaponDatas)
        {
            bool isUnlocked = UserSaveDataManager.Instance.IsWeaponUnlocked(weaponData.ID);
            bool isSelected = GlobalGameManager.Instance.SelectedWeaponData == weaponData;

            weaponSelectItemContexts.Add(new(weaponData, isUnlocked, isSelected));
        }

        _startUI.SetWeaponSelectItems(weaponSelectItemContexts);
    }

    public void Reset()
    {
        UnregisterEvents();
    }
    #endregion

    #region 이벤트 등록, 해제
    private void RegisterEvents()
    {
        UserSaveDataManager.Instance.OnDNAChanged += HandleDNAChanged;

        GlobalGameManager.Instance.OnSelectedPlayerDataChanged += HandleOnSelectedPlayerDataChanged;
        GlobalGameManager.Instance.OnSelectedWeaponDataChanged += HandleOnSelectedWeaponDataChanged;

        _startUI.OnStartButtonClicked += HandleOnStartButtonClicked;
        _startUI.OnCloseButtonClicked += HandleOnCloseButtonClicked;

        _startUI.OnPlayerSelectItemPointerEntered += HandleOnPlayerSelectItemPointerEntered;
        _startUI.OnPlayerSelectItemPointerExited += HandleOnPlayerSelectItemPointerExited;
        _startUI.OnPlayerSelectItemClicked += HandleOnPlayerSelectItemClicked;

        _startUI.OnWeaponSelectItemPointerEntered += HandleOnWeaponSelectItemPointerEntered;
        _startUI.OnWeaponSelectItemPointerExited += HandleOnWeaponSelectItemPointerExited;
        _startUI.OnWeaponSelectItemClicked += HandleOnWeaponSelectItemClicked;
    }

    private void UnregisterEvents()
    {
        UserSaveDataManager.Instance.OnDNAChanged -= HandleDNAChanged;

        GlobalGameManager.Instance.OnSelectedPlayerDataChanged -= HandleOnSelectedPlayerDataChanged;
        GlobalGameManager.Instance.OnSelectedWeaponDataChanged -= HandleOnSelectedWeaponDataChanged;

        _startUI.OnStartButtonClicked -= HandleOnStartButtonClicked;
        _startUI.OnCloseButtonClicked -= HandleOnCloseButtonClicked;

        _startUI.OnPlayerSelectItemPointerEntered -= HandleOnPlayerSelectItemPointerEntered;
        _startUI.OnPlayerSelectItemPointerExited -= HandleOnPlayerSelectItemPointerExited;
        _startUI.OnPlayerSelectItemClicked -= HandleOnPlayerSelectItemClicked;

        _startUI.OnWeaponSelectItemPointerEntered -= HandleOnWeaponSelectItemPointerEntered;
        _startUI.OnWeaponSelectItemPointerExited -= HandleOnWeaponSelectItemPointerExited;
        _startUI.OnWeaponSelectItemClicked -= HandleOnWeaponSelectItemClicked;
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleDNAChanged(int dnaAmount)
    {
        //DNA 텍스트 업데이트
        _startUI.UpdateDNAText(dnaAmount);
    }

    private void HandleOnSelectedPlayerDataChanged(PlayerData data)
    {
        //선택된 플레이어 업데이트
        _startUI.UpdateSelectedPlayer(data);
    }

    private void HandleOnSelectedWeaponDataChanged(WeaponData data)
    {
        //선택된 무기 업데이트
        _startUI.UpdateSelectedWeapon(data);
    }

    private void HandleOnStartButtonClicked()
    {
        //적용된 진화 정보 설정
        GlobalGameManager.Instance.SetAppliedEvolutions();

        //게임 씬으로 전환
        SceneTransitionManager.Instance.ChangeScene(SceneTransitionManager.GAME_SCENE_NAME);
    }

    private void HandleOnCloseButtonClicked()
    {
        Hide();
    }

    private void HandleOnPlayerSelectItemPointerEntered(PlayerData data)
    {
        //색 가져오기
        Color color = DataManager.Instance.RarityDataList.GetRarityColor(data.Rarity);

        //툴팁 요청
        OnTooltipRequested?.Invoke(new(
            data,
            data.Name,
            data.GetDescription(),
            color,
            data.Icon,
            data.BasePrice
        ));
    }

    private void HandleOnPlayerSelectItemPointerExited(PlayerData data)
    {
        //툴팁 요청 취소
        OnTooltipRequestCanceled?.Invoke(data);
    }

    private void HandleOnPlayerSelectItemClicked(PlayerData data)
    {
        bool isUnlocked = UserSaveDataManager.Instance.IsPlayerUnlocked(data.ID);

        if (isUnlocked)
        {
            //선택된 플레이어 데이터 설정
            GlobalGameManager.Instance.SetSelectedPlayerData(data);

            //마지막으로 선택한 플레이어 ID 저장
            UserSaveDataManager.Instance.SetLastSelectedPlayerID(data.ID);

            //저장
            UserSaveDataManager.Instance.SaveUserSaveData();
        }
        else
        {
            //구매 시도
            bool success = UserSaveDataManager.Instance.TrySpendDNA(data.BasePrice);

            if (success)
            {
                //플레이어 잠금 해제
                UserSaveDataManager.Instance.AddPlayerData(data.ID);

                //저장
                UserSaveDataManager.Instance.SaveUserSaveData();

                //UI 업데이트
                _startUI.UpdatePlayerSelectItem(new(data, true, false));
            }
            else
            {
                //TODO: 구매 실패 처리 (예: 알림 표시)
            }
        }
    }

    private void HandleOnWeaponSelectItemPointerEntered(WeaponData data)
    {
        //색 가져오기
        Color color = DataManager.Instance.RarityDataList.GetRarityColor(data.Rarity);

        //툴팁 요청
        OnTooltipRequested?.Invoke(new(
            data,
            data.Name,
            data.GetDescription(),
            color,
            data.Icon,
            data.BasePrice
        ));
    }

    private void HandleOnWeaponSelectItemPointerExited(WeaponData data)
    {
        //툴팁 요청 취소
        OnTooltipRequestCanceled?.Invoke(data);
    }

    private void HandleOnWeaponSelectItemClicked(WeaponData data)
    {
        bool isUnlocked = UserSaveDataManager.Instance.IsWeaponUnlocked(data.ID);

        if (isUnlocked)
        {
            //선택된 무기 데이터 설정
            GlobalGameManager.Instance.SetSelectedWeaponData(data);

            //마지막으로 선택한 무기 ID 저장
            UserSaveDataManager.Instance.SetLastSelectedWeaponID(data.ID);

            //저장
            UserSaveDataManager.Instance.SaveUserSaveData();
        }
        else
        {
            //구매 시도
            bool success = UserSaveDataManager.Instance.TrySpendDNA(data.BasePrice);

            if (success)
            {
                //무기 잠금 해제
                UserSaveDataManager.Instance.AddWeaponData(data.ID);

                //저장
                UserSaveDataManager.Instance.SaveUserSaveData();

                //UI 업데이트
                _startUI.UpdateWeaponSelectItem(new(data, true, false));
            }
            else
            {
                //TODO: 구매 실패 처리 (예: 알림 표시)
            }
        }
    }
    #endregion

    #region Show, Hide
    public void Show()
    {
        //UI 표시
        _startUI.Show(0f);

        //취소 가능 관리자에 등록
        _cancelableManager.PushCancelable(this);
    }

    public void Hide()
    {
        //UI 숨기기
        _startUI.Hide(0f);

        //취소 가능 관리자에서 해제
        _cancelableManager.PopCancelable(this);

        //닫힘 이벤트 호출
        OnClosed?.Invoke();
    }
    #endregion

    public void Cancel()
    {
        //닫기 버튼을 누른 것과 같이 처리
        HandleOnCloseButtonClicked();
    }
}