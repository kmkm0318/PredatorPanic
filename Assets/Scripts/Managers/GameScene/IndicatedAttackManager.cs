using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 적 공격 표시 관리자 클래스
/// </summary>
public class IndicatedAttackManager : MonoBehaviour
{
    #region 오브젝트 풀
    private Dictionary<IndicatedAttackData, ObjectPool<IndicatedAttack>> _indicatedAttacks = new();
    private List<IndicatedAttack> _activeIndicatedAttacks = new();
    #endregion

    #region 오브젝트 풀링
    private void InitPool(IndicatedAttackData data)
    {
        //이미 풀링이 되어있다면 패스
        if (_indicatedAttacks.ContainsKey(data)) return;

        //오브젝트 풀 생성
        ObjectPool<IndicatedAttack> pool = new(
            () =>
            {
                var attack = Instantiate(data.IndicatedAttackPrefab, transform);
                attack.Init(data, this);
                return attack;
            },
            (attack) => attack.gameObject.SetActive(true),
            (attack) => attack.gameObject.SetActive(false),
            (attack) => Destroy(attack.gameObject)
        );

        //딕셔너리에 추가
        _indicatedAttacks.Add(data, pool);
    }

    private ObjectPool<IndicatedAttack> GetPool(IndicatedAttackData data)
    {
        //풀링이 되어있지 않다면 초기화
        if (!_indicatedAttacks.TryGetValue(data, out var pool))
        {
            InitPool(data);
            pool = _indicatedAttacks[data];
        }

        return pool;
    }
    #endregion

    #region 생성 및 반환
    public void SpawnIndicatedAttack(IndicatedAttackData data, Vector3 position, float radius, float delay, float damage)
    {
        //풀 가져오기
        var pool = GetPool(data);

        //오브젝트 가져오기
        var attack = pool.Get();

        //공격 실행
        attack.StartAttack(position, radius, delay, damage);
    }

    public IndicatedAttack GetIndicatedAttack(IndicatedAttackData data)
    {
        //풀 가져오기
        var pool = GetPool(data);

        //오브젝트 가져오기
        var attack = pool.Get();

        //활성화
        attack.gameObject.SetActive(true);

        //활성화된 오브젝트 목록에 추가
        _activeIndicatedAttacks.Add(attack);

        return attack;
    }

    public void ReleaseIndicatedAttack(IndicatedAttack attack)
    {
        //풀 가져오기
        var pool = GetPool(attack.IndicatedAttackData);

        //오브젝트 반환
        pool.Release(attack);

        //활성화된 오브젝트 목록에서 제거
        _activeIndicatedAttacks.Remove(attack);
    }
    #endregion
}