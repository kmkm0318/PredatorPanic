using UnityEngine;

/// <summary>
/// 총 비주얼 컴포넌트
/// 발사 애니메이션 포함
/// </summary>
[RequireComponent(typeof(Animator))]
public class GunVisual : MonoBehaviour
{
    private Animator _animator;

    #region 애니메이션 문자열 및 해싱
    private const string FIRE = "Fire";
    private int FIRE_HASH = Animator.StringToHash(FIRE);
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayFire(float speed = 1f)
    {
        _animator.speed = speed;
        _animator.SetTrigger(FIRE_HASH);
    }
}