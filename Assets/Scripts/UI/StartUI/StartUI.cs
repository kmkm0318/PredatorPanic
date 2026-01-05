using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// 시작 화면 UI
/// 플레이어, 무기를 구입하거나 이미 구입한 플레이어, 무기를 통해 게임을 시작
/// </summary>
public class StartUI : ShowHideUI
{
    [Header("Run Select")]
    [SerializeField] private RunSelectItemUI _runSelectItemUIPrefab;
    [SerializeField] private Transform _runSelectItemUIParent;
    [SerializeField] private IconSlot _selectedRunIconSlot;

    [Header("Player Select")]
    [SerializeField] private PlayerSelectItemUI _playerSelectItemUIPrefab;
    [SerializeField] private Transform _playerSelectItemUIParent;
    [SerializeField] private IconSlot _selectedPlayerIconSlot;

    [Header("Weapon Select")]
    [SerializeField] private WeaponSelectItemUI _weaponSelectItemUIPrefab;
    [SerializeField] private Transform _weaponSelectItemUIParent;
    [SerializeField] private IconSlot _selectedWeaponIconSlot;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _dnaText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _closeButton;

    #region 오브젝트 풀
    private ObjectPool<RunSelectItemUI> _runSelectItemUIPool;
    private ObjectPool<PlayerSelectItemUI> _playerSelectItemUIPool;
    private ObjectPool<WeaponSelectItemUI> _weaponSelectItemUIPool;
    #endregion

    #region 이벤트
    public event Action OnStartButtonClicked;
    public event Action OnCloseButtonClicked;
    public event Action<RunData> OnRunSelectItemClicked;
    public event Action<RunData> OnRunSelectItemPointerEntered;
    public event Action<RunData> OnRunSelectItemPointerExited;
    public event Action<PlayerData> OnPlayerSelectItemClicked;
    public event Action<PlayerData> OnPlayerSelectItemPointerEntered;
    public event Action<PlayerData> OnPlayerSelectItemPointerExited;
    public event Action<WeaponData> OnWeaponSelectItemClicked;
    public event Action<WeaponData> OnWeaponSelectItemPointerEntered;
    public event Action<WeaponData> OnWeaponSelectItemPointerExited;
    #endregion

    private void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        //시작 버튼 이벤트 구독
        _startButton.onClick.AddListener(() =>
        {
            OnStartButtonClicked?.Invoke();
        });

        //닫기 버튼 이벤트 구독
        _closeButton.onClick.AddListener(() =>
        {
            OnCloseButtonClicked?.Invoke();
        });
    }

    #region 오브젝트 풀링
    private void InitPool()
    {
        _runSelectItemUIPool = new(
            () => Instantiate(_runSelectItemUIPrefab, _runSelectItemUIParent),
            item =>
            {
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
            },
            item => item.gameObject.SetActive(false),
            item => Destroy(item.gameObject)
        );

        _playerSelectItemUIPool = new(
            () => Instantiate(_playerSelectItemUIPrefab, _playerSelectItemUIParent),
            item =>
            {
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
            },
            item => item.gameObject.SetActive(false),
            item => Destroy(item.gameObject)
        );

        _weaponSelectItemUIPool = new(
            () => Instantiate(_weaponSelectItemUIPrefab, _weaponSelectItemUIParent),
            item =>
            {
                item.gameObject.SetActive(true);
                item.transform.SetAsLastSibling();
            },
            item => item.gameObject.SetActive(false),
            item => Destroy(item.gameObject)
        );
    }
    #endregion

    #region UI 업데이트
    public void UpdateDNAText(int dnaAmount)
    {
        _dnaText.text = dnaAmount.ToString();
    }

    public void UpdateSelectedRun(RunData runData)
    {
        //유효성 검사
        if (runData == null) return;

        //아이콘 변경
        _selectedRunIconSlot.SetIcon(runData.Icon);

        //색 가져오기
        var color = DataManager.Instance.RarityDataList.GetRarityColor(runData.Rarity);

        //색상 변경
        _selectedRunIconSlot.SetColor(color);

        //모든 아이콘 업데이트
        foreach (Transform child in _runSelectItemUIParent)
        {
            if (child.TryGetComponent<RunSelectItemUI>(out var itemUI))
            {
                //선택된 아이템만 true
                itemUI.UpdateSelected(itemUI.RunData == runData);
            }
        }
    }

    public void UpdateSelectedPlayer(PlayerData playerData)
    {
        //유효성 검사
        if (playerData == null) return;

        //아이콘 변경
        _selectedPlayerIconSlot.SetIcon(playerData.Icon);

        //색 가져오기
        var color = DataManager.Instance.RarityDataList.GetRarityColor(playerData.Rarity);

        //색상 변경
        _selectedPlayerIconSlot.SetColor(color);

        //모든 아이콘 업데이트
        foreach (Transform child in _playerSelectItemUIParent)
        {
            if (child.TryGetComponent<PlayerSelectItemUI>(out var itemUI))
            {
                //선택된 아이템만 true
                itemUI.UpdateSelected(itemUI.PlayerData == playerData);
            }
        }
    }

    public void UpdateSelectedWeapon(WeaponData weaponData)
    {
        //유효성 검사
        if (weaponData == null) return;

        //아이콘 변경
        _selectedWeaponIconSlot.SetIcon(weaponData.Icon);

        //색 가져오기
        var color = DataManager.Instance.RarityDataList.GetRarityColor(weaponData.Rarity);

        //색상 변경
        _selectedWeaponIconSlot.SetColor(color);

        //모든 아이콘 업데이트
        foreach (Transform child in _weaponSelectItemUIParent)
        {
            if (child.TryGetComponent<WeaponSelectItemUI>(out var itemUI))
            {
                //선택된 아이템만 true
                itemUI.UpdateSelected(itemUI.WeaponData == weaponData);
            }
        }
    }

    public void SetRunSelectItems(List<RunSelectItemContext> runSelectItemContexts)
    {
        //풀 초기화
        if (_runSelectItemUIPool == null) InitPool();

        //기존 아이템 UI 반환
        while (_runSelectItemUIParent.childCount > 0)
        {
            //자식 가져오기
            var child = _runSelectItemUIParent.GetChild(0);

            if (child.TryGetComponent<RunSelectItemUI>(out var itemUI))
            {
                //이벤트 해제
                itemUI.OnPointerEntered -= HandleOnRunSelectItemPointerEntered;
                itemUI.OnPointerExited -= HandleOnRunSelectItemPointerExited;
                itemUI.OnPointerClicked -= HandleOnRunSelectItemPointerClicked;

                //오브젝트 풀에 반환
                _runSelectItemUIPool.Release(itemUI);
            }
            else
            {
                //존재하지 않으면 파괴
                Destroy(child.gameObject);
            }
        }

        //런 아이템 UI 생성 및 초기화
        foreach (var context in runSelectItemContexts)
        {
            //오브젝트 풀에서 가져오기
            var itemUI = _runSelectItemUIPool.Get();

            //초기화
            itemUI.Init(context.RunData, context.IsUnlocked, context.IsSelected);

            //이벤트 구독
            itemUI.OnPointerEntered += HandleOnRunSelectItemPointerEntered;
            itemUI.OnPointerExited += HandleOnRunSelectItemPointerExited;
            itemUI.OnPointerClicked += HandleOnRunSelectItemPointerClicked;
        }
    }

    public void SetPlayerSelectItems(List<PlayerSelectItemContext> playerSelectItemContexts)
    {
        //풀 초기화
        if (_playerSelectItemUIPool == null) InitPool();

        //기존 아이템 UI 반환
        while (_playerSelectItemUIParent.childCount > 0)
        {
            //자식 가져오기
            var child = _playerSelectItemUIParent.GetChild(0);

            if (child.TryGetComponent<PlayerSelectItemUI>(out var itemUI))
            {
                //이벤트 해제
                itemUI.OnPointerEntered -= HandleOnPlayerSelectItemPointerEntered;
                itemUI.OnPointerExited -= HandleOnPlayerSelectItemPointerExited;
                itemUI.OnPointerClicked -= HandleOnPlayerSelectItemPointerClicked;

                //오브젝트 풀에 반환
                _playerSelectItemUIPool.Release(itemUI);
            }
            else
            {
                //존재하지 않으면 파괴
                Destroy(child.gameObject);
            }
        }

        //진화 아이템 UI 생성 및 초기화
        foreach (var context in playerSelectItemContexts)
        {
            //오브젝트 풀에서 가져오기
            var itemUI = _playerSelectItemUIPool.Get();

            //초기화
            itemUI.Init(context.PlayerData, context.IsUnlocked, context.IsSelected);

            //이벤트 구독
            itemUI.OnPointerEntered += HandleOnPlayerSelectItemPointerEntered;
            itemUI.OnPointerExited += HandleOnPlayerSelectItemPointerExited;
            itemUI.OnPointerClicked += HandleOnPlayerSelectItemPointerClicked;
        }
    }

    public void UpdateRunSelectItem(RunSelectItemContext context)
    {
        //모든 아이템 UI 순회
        foreach (Transform child in _runSelectItemUIParent)
        {
            if (child.TryGetComponent<RunSelectItemUI>(out var itemUI))
            {
                //해당 런 데이터와 일치하는 아이템 UI 찾기
                if (itemUI.RunData == context.RunData)
                {
                    //업데이트
                    itemUI.Init(context.RunData, context.IsUnlocked, context.IsSelected);
                    break;
                }
            }
        }
    }

    public void UpdatePlayerSelectItem(PlayerSelectItemContext context)
    {
        //모든 아이템 UI 순회
        foreach (Transform child in _playerSelectItemUIParent)
        {
            if (child.TryGetComponent<PlayerSelectItemUI>(out var itemUI))
            {
                //해당 플레이어 데이터와 일치하는 아이템 UI 찾기
                if (itemUI.PlayerData == context.PlayerData)
                {
                    //업데이트
                    itemUI.Init(context.PlayerData, context.IsUnlocked, context.IsSelected);
                    break;
                }
            }
        }
    }

    public void SetWeaponSelectItems(List<WeaponSelectItemContext> weaponSelectItemContexts)
    {
        //풀 초기화
        if (_weaponSelectItemUIPool == null) InitPool();

        //기존 아이템 UI 반환
        while (_weaponSelectItemUIParent.childCount > 0)
        {
            //자식 가져오기
            var child = _weaponSelectItemUIParent.GetChild(0);

            if (child.TryGetComponent<WeaponSelectItemUI>(out var itemUI))
            {
                //이벤트 해제
                itemUI.OnPointerEntered -= HandleOnWeaponSelectItemPointerEntered;
                itemUI.OnPointerExited -= HandleOnWeaponSelectItemPointerExited;
                itemUI.OnPointerClicked -= HandleOnWeaponSelectItemPointerClicked;

                //오브젝트 풀에 반환
                _weaponSelectItemUIPool.Release(itemUI);
            }
            else
            {
                //존재하지 않으면 파괴
                Destroy(child.gameObject);
            }
        }

        //무기 아이템 UI 생성 및 초기화
        foreach (var context in weaponSelectItemContexts)
        {
            //오브젝트 풀에서 가져오기
            var itemUI = _weaponSelectItemUIPool.Get();

            //초기화
            itemUI.Init(context.WeaponData, context.IsUnlocked, context.IsSelected);

            //이벤트 구독
            itemUI.OnPointerEntered += HandleOnWeaponSelectItemPointerEntered;
            itemUI.OnPointerExited += HandleOnWeaponSelectItemPointerExited;
            itemUI.OnPointerClicked += HandleOnWeaponSelectItemPointerClicked;
        }
    }

    public void UpdateWeaponSelectItem(WeaponSelectItemContext context)
    {
        //모든 아이템 UI 순회
        foreach (Transform child in _weaponSelectItemUIParent)
        {
            if (child.TryGetComponent<WeaponSelectItemUI>(out var itemUI))
            {
                //해당 무기 데이터와 일치하는 아이템 UI 찾기
                if (itemUI.WeaponData == context.WeaponData)
                {
                    //업데이트
                    itemUI.Init(context.WeaponData, context.IsUnlocked, context.IsSelected);
                    break;
                }
            }
        }
    }
    #endregion

    #region 이벤트 핸들러
    private void HandleOnRunSelectItemPointerEntered(RunData data)
    {
        OnRunSelectItemPointerEntered?.Invoke(data);
    }

    private void HandleOnRunSelectItemPointerExited(RunData data)
    {
        OnRunSelectItemPointerExited?.Invoke(data);
    }

    private void HandleOnRunSelectItemPointerClicked(RunData data)
    {
        OnRunSelectItemClicked?.Invoke(data);
    }

    private void HandleOnPlayerSelectItemPointerEntered(PlayerData data)
    {
        OnPlayerSelectItemPointerEntered?.Invoke(data);
    }

    private void HandleOnPlayerSelectItemPointerExited(PlayerData data)
    {
        OnPlayerSelectItemPointerExited?.Invoke(data);
    }

    private void HandleOnPlayerSelectItemPointerClicked(PlayerData data)
    {
        OnPlayerSelectItemClicked?.Invoke(data);
    }

    private void HandleOnWeaponSelectItemPointerEntered(WeaponData data)
    {
        OnWeaponSelectItemPointerEntered?.Invoke(data);
    }

    private void HandleOnWeaponSelectItemPointerExited(WeaponData data)
    {
        OnWeaponSelectItemPointerExited?.Invoke(data);
    }

    private void HandleOnWeaponSelectItemPointerClicked(WeaponData data)
    {
        OnWeaponSelectItemClicked?.Invoke(data);
    }
    #endregion
}