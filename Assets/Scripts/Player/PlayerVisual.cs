using DG.Tweening;
using UnityEngine;

/// <summary>
/// 플레이어 비주얼 컴포넌트
/// 카메라 피벗과 무기 피벗 포함
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    #region 컴포넌트
    public Animator Animator { get; private set; }
    #endregion

    #region 애니메이션 해시
    public static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    public static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    public static readonly int IsFallingHash = Animator.StringToHash("isFalling");
    #endregion

    #region 무적 상태 시 EmissionColor 변경
    private static readonly int _emissionColorHash = Shader.PropertyToID("_EmissionColor");
    private static readonly Color _invincibleColor = Color.gray;
    private Renderer[] _renderers;
    private MaterialPropertyBlock _mpb;
    #endregion

    #region 스폰 애니메이션
    private Tween _spawnTween;
    #endregion

    private void Awake()
    {
        //애니메이터 가져오기
        Animator = GetComponent<Animator>();

        //자식 오브젝트의 모든 렌더러 수집
        _renderers = GetComponentsInChildren<Renderer>();

        //mpb 초기화
        _mpb = new();
    }

    private void OnEnable()
    {
        //무적 상태 비주얼 초기화
        SetInvincibleVisual(false);
    }

    #region 무적 상태 비주얼 처리
    public void SetInvincibleVisual(bool isInvincible)
    {
        //모든 렌더러에 대해 머티리얼 프로퍼티 블럭 설정
        foreach (var renderer in _renderers)
        {
            //MPB 가져옥;
            renderer.GetPropertyBlock(_mpb);

            //Invincible 설정
            _mpb.SetColor(_emissionColorHash, isInvincible ? _invincibleColor : Color.black);

            //MPB 다시 설정
            renderer.SetPropertyBlock(_mpb);
        }
    }
    #endregion

    #region 스폰 애니메이션
    public void PlaySpawnAnimation(float spawnVisualOffsetY, float spawnVisualDuration, Ease spawnVisualEase)
    {
        //이전 트윈이 있으면 종료
        _spawnTween?.Kill();

        //스폰 애니메이션 실행
        _spawnTween = transform.DOLocalMoveY(0f, spawnVisualDuration)
            .From(spawnVisualOffsetY)
            .SetEase(spawnVisualEase)
            .OnComplete(() =>
            {
                //이동 완료 시 위치 초기화
                transform.localPosition = Vector3.zero;
            });
    }
    #endregion
}