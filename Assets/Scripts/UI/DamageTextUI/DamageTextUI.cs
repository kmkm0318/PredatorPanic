using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 데미지 텍스트 UI 매니저
/// </summary>
public class DamageTextUI : MonoBehaviour
{
    #region 오브젝트 풀
    private Dictionary<DamageTextData, ObjectPool<DamageText>> _pools = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool(DamageTextData data)
    {
        ObjectPool<DamageText> pool = new(
            () => Instantiate(data.DamageTextPrefab, transform),
            (damageText) => { damageText.gameObject.SetActive(true); },
            (damageText) => { damageText.gameObject.SetActive(false); },
            (damageText) => { Destroy(damageText.gameObject); }
        );

        _pools[data] = pool;
    }

    private ObjectPool<DamageText> GetPool(DamageTextData data)
    {
        if (!_pools.TryGetValue(data, out var pool))
        {
            InitPool(data);
            pool = _pools[data];
        }

        return pool;
    }
    #endregion

    private void Awake()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        DamageText.OnAnyDamageTextDone += OnAnyDamageTextDone;
    }

    private void UnregisterEvents()
    {
        DamageText.OnAnyDamageTextDone -= OnAnyDamageTextDone;
    }
    #endregion

    #region 이벤트 핸들러
    // 데미지 텍스트 완료 시 오브젝트 풀에 반환
    private void OnAnyDamageTextDone(DamageText text)
    {
        var data = text.Data;
        var pool = GetPool(data);
        pool?.Release(text);
    }
    #endregion

    /// <summary>
    /// 데미지 텍스트 표시 함수
    /// </summary>
    public void ShowDamageText(Vector3 pos, Vector3 forward, float damage, bool isCritical, DamageTextType type = DamageTextType.Normal)
    {
        //데미지 텍스트 데이터 리스트 가져오기
        var dataList = DataManager.Instance.DamageTextDataList;

        //데미지 텍스트 데이터 가져오기
        if (!dataList.DamageTextDataDict.TryGetValue(type, out var data))
        {
            $"DamageTextData not found. type: {type}".LogError();
            return;
        }

        //오브젝트 풀에서 데미지 텍스트 가져오기
        var pool = GetPool(data);
        var damageText = pool.Get();

        //데미지 텍스트 위치, 방향 설정
        damageText.transform.position = pos;
        damageText.transform.forward = forward;

        //데미지 텍스트 표시
        damageText.Show(data, damage, isCritical);
    }
}