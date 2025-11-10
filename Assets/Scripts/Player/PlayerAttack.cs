using UnityEngine;

/// <summary>
/// 플레이어 공격 컴포넌트
/// 무기를 장착하고 공격 명령을 받아 공격 실행
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    private Weapon _weapon;

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void StartAttack()
    {
        if (_weapon != null)
        {
            _weapon.StartAttack();
        }
    }

    public void StopAttack()
    {
        if (_weapon != null)
        {
            _weapon.StopAttack();
        }
    }
}