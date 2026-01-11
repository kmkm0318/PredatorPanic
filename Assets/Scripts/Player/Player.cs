using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 플레이어 클래스
/// 플레이어 데이터와 컴포넌트 초기화 담당
/// 각 컴포넌트 간의 연결 관리
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerMagnet))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    #region 에디터 참조 변수
    [Header("Player")]
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private Transform _playerCenter;
    public PlayerVisual PlayerVisual => _playerVisual;
    public Vector3 CenterPosition => _playerCenter.position;
    #endregion

    #region 플레이어 데이터
    public PlayerData PlayerData { get; private set; }
    #endregion

    #region 레퍼런스
    public GameManager GameManager { get; private set; }
    #endregion

    #region 컴포넌트
    private PlayerController _playerController;
    private PlayerAttack _playerAttack;
    private PlayerMagnet _playerItemCollector;
    public PlayerHealth PlayerHealth { get; private set; }
    #endregion

    #region 플레이어 스탯
    private Stats<PlayerStatType> _playerStats;
    public Stats<PlayerStatType> PlayerStats => _playerStats;
    #endregion

    #region 프로퍼티
    public List<Weapon> Weapons => _playerAttack.Weapons;
    #endregion

    #region 효과 주체 리스트
    public List<LevelUpReward> LevelUpRewards { get; private set; } = new();
    public List<Item> Items { get; private set; } = new();
    public List<Evolution> Evolutions { get; private set; } = new();
    #endregion

    #region 레벨, 경험치
    public int Level { get; private set; }
    public float CurExp { get; private set; }
    public float MaxExp { get; private set; }
    #endregion

    #region 플레이어 재화(이빨, DNA)
    public int Tooth { get; private set; } = 0;
    public int DNA { get; private set; } = 0;
    #endregion

    #region 이벤트
    public event Action<int> OnLevelChanged; //레벨 변경 이벤트. UI 갱신에 사용
    public event Action<int> OnLevelUpped; //레벨 업 이벤트. 게임 로직에 사용
    public event Action<float, float> OnExpChanged;
    public event Action<int> OnToothChanged;
    public event Action<int> OnDNAChanged;
    public event Action<PlayerDamageContext> OnHit; //적중 이벤트
    public event Action<PlayerDamageContext> OnKill; //킬 이벤트
    public event Action<List<Weapon>> OnWeaponsChanged; //무기 리스트 변경 이벤트
    public event Action<List<Item>> OnItemsChanged; //아이템 리스트 변경 이벤트
    public event Action OnDeath; //플레이어 사망 이벤트
    #endregion

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerItemCollector = GetComponent<PlayerMagnet>();
        PlayerHealth = GetComponent<PlayerHealth>();
    }

    #region 초기화
    public void Init(PlayerData playerData, GameManager gameManager)
    {
        PlayerData = playerData;
        GameManager = gameManager;

        //컴포넌트에서 스탯을 사용하기 때문에 스탯을 먼저 초기화
        InitPlayer();
        InitComponents();
    }

    // 플레이어 초기화
    // 스탯 초기화 및 기본 값 설정
    // 캐릭터 효과 적용
    private void InitPlayer()
    {
        //기본 스탯 데이터 가져오기
        var baseStats = PlayerData.PlayerBaseStatsData.BaseStats;

        //스탯 객체 생성
        _playerStats = new(baseStats);

        //캐릭터 효과 가져오기
        var characterEffectDatas = PlayerData.CharacterEffectDatas;

        //효과 데이터로부터 효과 적용
        foreach (var effectData in characterEffectDatas)
        {
            //이펙트 객체 생성
            var effect = effectData.GetEffect();

            //효과 적용
            effect.Apply(this);
        }

        //초기 레벨과 현재 경험치 설정
        Level = 0;
        CurExp = 0;

        //최대 경험치 자동 계산
        UpdateMaxExp();
    }

    // 컴포넌트 초기화
    private void InitComponents()
    {
        _playerController.Init(this);
        _playerItemCollector.Init(this);
        PlayerHealth.Init(this);

        PlayerHealth.OnInvincibleStateChanged += HandleOnInvincibleStateChanged;
        PlayerHealth.OnTakeDamage += HandleOnTakeDamage;
        PlayerHealth.OnDeath += HandleOnDeath;
    }

    #region 플레이어 체력 이벤트 핸들러
    private void HandleOnInvincibleStateChanged(bool isInvincible)
    {
        //비주얼에 무적 상태 반영
        _playerVisual.SetInvincibleVisual(isInvincible);
    }

    private void HandleOnTakeDamage(float damage)
    {
        //플레이어 히트 효과음 재생
        AudioManager.Instance.PlaySfx(PlayerData.HitAudioData, CenterPosition);

        //플레이어 히트 이펙트 재생
        ParticleEffectManager.Instance.Play(PlayerData.HitParticleEffectData, CenterPosition, transform.rotation);

        //카메라 흔들림 재생
        GameManager.CameraManager.PlayDamageImpulse();
    }

    private void HandleOnDeath()
    {
        //플레이어 오브젝트 숨기기
        gameObject.SetActive(false);

        //플레이어 사망 효과음 재생
        AudioManager.Instance.PlaySfx(PlayerData.DeathAudioData, CenterPosition);

        //플레이어 사망 이펙트 재생
        ParticleEffectManager.Instance.Play(PlayerData.DeathParticleEffectData, CenterPosition, transform.rotation);

        //이벤트 호출
        OnDeath?.Invoke();
    }
    #endregion

    // 스폰 시 애니메이션 재생
    public void OnSpawn()
    {
        _playerVisual.PlaySpawnAnimation(PlayerData.SpawnVisualOffsetY, PlayerData.SpawnVisualDuration, PlayerData.SpawnVisualEase);
    }
    #endregion

    #region 무기
    // WeaponData로 무기 추가
    public bool TryAddWeapon(WeaponData weaponData)
    {
        //유효성 검사
        if (weaponData == null)
        {
            "무기 데이터가 올바르지 않습니다.".LogWarning();
            return false;
        }

        //최대 무기 개수 검사
        var curWeaponCount = _playerAttack.Weapons.Count;
        if (curWeaponCount >= PlayerData.WeaponCountMax)
        {
            "최대 무기 개수 초과".Log();
            return false;
        }

        //무기 생성 및 추가
        var weapon = weaponData.GetWeapon();
        weapon.OnEquip(this);
        _playerAttack.AddWeapon(weapon);

        //무기 변경 이벤트 발생
        OnWeaponsChanged?.Invoke(_playerAttack.Weapons);

        return true;
    }

    /// <summary>
    /// 무기 추가 가능 여부 확인
    /// </summary>
    public bool CanAddWeapon()
    {
        return _playerAttack.Weapons.Count < PlayerData.WeaponCountMax;
    }

    //Weapon으로 무기 제거
    public void RemoveWeapon(Weapon weapon)
    {
        var idx = _playerAttack.RemoveWeapon(weapon);

        //무기 제거 성공 시 피벗과 함께 무기 오브젝트 제거
        if (idx >= 0)
        {
            //무기 변경 이벤트 발생
            OnWeaponsChanged?.Invoke(_playerAttack.Weapons);
        }
    }
    #endregion

    #region 카메라
    //시네머신 카메라 초기화
    public void InitCamera(CinemachineCamera camera)
    {
        //카메라가 null이면 패스
        if (camera == null)
        {
            $"Camera is null. Player: {name}".LogWarning();
            return;
        }

        //카메라 타겟 설정
        camera.Follow = _playerCenter;
        camera.LookAt = _playerCenter;
    }
    #endregion

    #region 공격
    // 공격 시작, 종료
    public void StartAttack()
    {
        _playerAttack.StartAttack();
    }

    public void StopAttack()
    {
        _playerAttack.StopAttack();
    }

    public void HandleOnHit(in PlayerDamageContext context)
    {
        //생명력 흡수 적용
        float lifeSteal = _playerStats.GetStat(PlayerStatType.LifeSteal).FinalValue;
        if (lifeSteal > 0f)
        {
            float healAmount = context.Damage * lifeSteal;
            PlayerHealth.Heal(healAmount);
        }

        //적중 이벤트 발생
        OnHit?.Invoke(context);
    }

    public void HandleOnKill(PlayerDamageContext context)
    {
        //킬 이벤트 발생
        OnKill?.Invoke(context);
    }
    #endregion

    #region 레벨, 경험치
    // 최대 경험치 업데이트
    private void UpdateMaxExp()
    {
        //경험치 데이터 가져오기
        var expData = PlayerData.PlayerExpData;

        //최대 경험치 계산
        float baseExp = expData.BaseExp;
        float expGrowthRate = expData.ExpGrowthRate;
        MaxExp = baseExp * Mathf.Pow(expGrowthRate, Level);
    }

    //경험치 획득. 레벨업은 따로 처리
    public void AddExp(float amount)
    {
        float expGrainRate = _playerStats.GetStat(PlayerStatType.EXPGainRate).FinalValue;
        float expAmount = amount * expGrainRate;

        CurExp += expAmount;
        OnExpChanged?.Invoke(CurExp, MaxExp);
    }

    //레벨업을 Playing, RoundClear 등에서 시도
    public bool TryLevelUp()
    {
        if (CurExp < MaxExp) return false;

        CurExp -= MaxExp;
        Level++;
        UpdateMaxExp();
        OnLevelChanged?.Invoke(Level);
        OnLevelUpped?.Invoke(Level);
        OnExpChanged?.Invoke(CurExp, MaxExp);

        return true;
    }
    #endregion

    #region 재화 획득 및 사용 시도
    public void AddTooth(int amount, bool applyGainRate = true)
    {
        if (applyGainRate)
        {
            float toothGainRate = _playerStats.GetStat(PlayerStatType.ToothGainRate).FinalValue;
            amount = Mathf.FloorToInt(amount * toothGainRate);
        }

        Tooth += amount;
        OnToothChanged?.Invoke(Tooth);
    }

    public bool TrySpendTooth(int amount)
    {
        if (Tooth < amount) return false;

        Tooth -= amount;
        OnToothChanged?.Invoke(Tooth);
        return true;
    }

    public void AddDNA(int amount, bool applyGainRate = true)
    {
        if (applyGainRate)
        {
            float dnaGainRate = _playerStats.GetStat(PlayerStatType.DNAGainRate).FinalValue;
            amount = Mathf.FloorToInt(amount * dnaGainRate);
        }

        DNA += amount;
        OnDNAChanged?.Invoke(DNA);
    }

    public bool TrySpendDNA(int amount)
    {
        if (DNA < amount) return false;

        DNA -= amount;
        OnDNAChanged?.Invoke(DNA);
        return true;
    }
    #endregion

    #region 레벨업 보상 효과 적용 및 해제
    //레벨업 보상 효과 적용
    public void ApplyLevelUpRewards(LevelUpRewardData data)
    {
        var reward = new LevelUpReward(data);
        reward.Apply(this);
        LevelUpRewards.Add(reward);
    }

    //레벨업 보상 효과 해제
    //사용할 예정 없음
    public void RemoveLevelUpRewards(LevelUpReward reward)
    {
        if (LevelUpRewards.Contains(reward))
        {
            reward.Remove(this);
            LevelUpRewards.Remove(reward);
        }
    }
    #endregion

    #region 아이템 장착 및 해제
    //아이템 장착. 현재는 아이템 제한 없음
    public bool TryEquipItem(ItemData itemData)
    {
        if (itemData == null) return false;

        //아이템 생성 및 추가
        var item = new Item(itemData);
        Items.Add(item);
        item.OnEquip(this);

        //아이템 변경 이벤트 발생
        OnItemsChanged?.Invoke(Items);

        return true;
    }

    //아이템 장착 해제
    public void UnequipItem(Item item)
    {
        if (item == null) return;

        //아이템이 장착된 상태인지 확인
        if (Items.Contains(item))
        {
            //아이템 해제 및 리스트에서 제거
            item.OnUnequip(this);
            Items.Remove(item);

            //아이템 변경 이벤트 발생
            OnItemsChanged?.Invoke(Items);
        }
    }
    #endregion

    #region 진화
    public void AddEvolution(EvolutionData evolutionData, int level)
    {
        //유효성 검사
        if (evolutionData == null) return;

        //레벨이 0이면 패스
        if (level <= 0) return;

        //진화 생성
        var evolution = new Evolution(evolutionData, level);

        //진화 추가
        Evolutions.Add(evolution);

        //효과 적용
        evolution.ApplyEffect(this);
    }

    public void RemoveEvolution(Evolution evolution)
    {
        if (evolution == null) return;

        //진화가 적용된 상태인지 확인
        if (Evolutions.Contains(evolution))
        {
            //효과 제거
            evolution.RemoveEffect(this);

            //진화 제거
            Evolutions.Remove(evolution);
        }
    }
    #endregion

    #region 체력 관리
    /// <summary>
    /// 플레이어 체력 완전 회복
    /// 게임 시작 시에 최대 체력을 맞추기 위해 사용
    /// </summary>
    public void HealFull()
    {
        PlayerHealth.Heal(PlayerHealth.MaxHealth);
    }

    /// <summary>
    /// 플레이어 체력 완전 소진
    /// 게임 종료 시에 사용
    /// </summary>
    public void DamageFull()
    {
        PlayerHealth.TakeDamage(PlayerHealth.CurrentHealth);
    }

    public void SetInvincible(bool isInvincible)
    {
        PlayerHealth.SetInvincible(isInvincible);
    }
    #endregion

    /// <summary>
    /// 예측 위치 반환
    /// </summary>
    public Vector3 GetPredictedPosition(float delayTime = 0f)
    {
        //플레이어 컨트롤러에 위임
        return _playerController.GetPredictedPosition(delayTime);
    }
}