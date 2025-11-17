using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 플레이어 클래스
/// 플레이어 데이터와 컴포넌트 초기화 담당
/// 각 컴포넌트 간의 연결 관리
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    #region 플레이어 비주얼 객체
    [SerializeField] private PlayerVisual _playerVisual;
    #endregion

    #region 플레이어 데이터
    private PlayerData _playerData;
    #endregion

    #region 컴포넌트
    private PlayerController _playerController;
    private PlayerAttack _playerAttack;
    private Health _health;
    #endregion

    #region 플레이어 스탯
    private Stats<PlayerStatType> _playerStats;
    public Stats<PlayerStatType> PlayerStats => _playerStats;
    #endregion

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAttack = GetComponent<PlayerAttack>();
        _health = GetComponent<Health>();
    }

    public void Init(PlayerData playerData, WeaponData weaponData)
    {
        _playerData = playerData;

        InitStats();
        InitComponents();
        AddWeapon(weaponData);
    }

    private void InitStats()
    {
        _playerStats = new(_playerData.InitialStats);
    }

    // 컴포넌트 초기화
    private void InitComponents()
    {
        _playerController.Init(this, _playerData.PlayerControllerData, _playerVisual);

        var maxHealth = _playerStats.GetStat(PlayerStatType.Health).FinalValue;
        var defense = _playerStats.GetStat(PlayerStatType.Defense).FinalValue;
        _health.Init(maxHealth, defense);
    }

    #region 무기
    // WeaponData로 무기 추가
    public void AddWeapon(WeaponData weaponData)
    {
        var curWeaponCount = _playerAttack.WeaponCount;
        if (curWeaponCount >= _playerData.WeaponCountMax)
        {
            Debug.LogWarning("최대 무기 개수 초과");
            return;
        }

        var weaponPivot = _playerVisual.GetWeaponPivot(curWeaponCount);
        if (weaponPivot == null)
        {
            Debug.LogWarning("무기 피벗이 null입니다.");
            return;
        }

        if (weaponData == null || weaponData.WeaponPrefab == null)
        {
            Debug.LogWarning("무기 데이터가 올바르지 않습니다.");
            return;
        }

        var weapon = Instantiate(weaponData.WeaponPrefab, weaponPivot);
        weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        weapon.Init(weaponData, this);
        _playerAttack.AddWeapon(weapon);
    }

    //Weapon으로 무기 제거
    public void RemoveWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("제거할 무기가 null입니다.");
            return;
        }

        bool removed = _playerAttack.RemoveWeapon(weapon);
        if (removed)
        {
            Destroy(weapon.gameObject);
        }
        else
        {
            Debug.LogWarning("무기 제거에 실패했습니다.");
        }
    }
    #endregion

    // 시네머신 카메라의 팔로우 타겟 설정
    public void SetCameraFollowTarget(CinemachineCamera camera)
    {
        if (camera != null)
        {
            camera.Follow = _playerVisual.CameraPivot;
        }
    }

    // 공격 시작, 종료
    public void StartAttack()
    {
        _playerAttack.StartAttack();
    }

    public void StopAttack()
    {
        _playerAttack.StopAttack();
    }
}