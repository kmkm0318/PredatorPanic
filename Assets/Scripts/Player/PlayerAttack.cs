using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 공격 컴포넌트
/// 무기를 장착하고 공격 명령을 받아 공격 실행
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    #region 변수들
    public List<Weapon> Weapons { get; private set; } = new();
    private bool _isAttacking = false;
    #endregion

    #region 무기 추가, 제거
    //Weapon으로 무기 추가
    public void AddWeapon(Weapon weapon)
    {
        Weapons.Add(weapon);
    }

    //Weapon으로 무기 제거. 무기 인덱스 반환
    public int RemoveWeapon(Weapon weapon)
    {
        int idx = Weapons.IndexOf(weapon);
        if (idx >= 0)
        {
            Weapons.RemoveAt(idx);
        }
        return idx;
    }

    public void StartAttack()
    {
        _isAttacking = true;
    }

    public void StopAttack()
    {
        _isAttacking = false;
    }
    #endregion

    private void Update()
    {
        HandleAttack();
    }

    /// <summary>
    /// 공격 처리 메서드
    /// </summary>
    private void HandleAttack()
    {
        //공격 중이 아닐 시 패스
        if (!_isAttacking) return;

        //장착된 모든 무기 공격 처리
        for (int i = 0; i < Weapons.Count; i++)
        {
            Weapons[i].HandleAttack();
        }
    }
}