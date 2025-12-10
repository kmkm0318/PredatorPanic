using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _tweenDuration = 0.5f;
    [SerializeField] private Ease _easeType = Ease.OutCubic;
    [SerializeField] private TMP_Text _valueText;

    private Tween _currentTween;

    public void SetValue(float cur, float max, bool instant = false)
    {
        if (_slider == null) return;

        _slider.maxValue = Mathf.Max(1e-6f, max);

        _currentTween?.Kill();

        if (instant)
        {
            _slider.value = cur;
        }
        else
        {
            _currentTween = _slider
            .DOValue(cur, _tweenDuration)
            .SetEase(_easeType);
        }

        UpdateText(cur, max);
    }

    public void SetValue(float value, bool instant = false)
    {
        SetValue(value, _slider.maxValue, instant);
    }

    private void UpdateText(float cur, float max)
    {
        if (_valueText == null) return;

        _valueText.text = $"{Mathf.CeilToInt(cur)} / {Mathf.CeilToInt(max)}";
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
    }
}