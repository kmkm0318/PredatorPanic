using System;

public interface IShowHide
{
    void Show(float duration = 0.5f, Action onComplete = null);
    void Hide(float duration = 0.5f, Action onComplete = null);
}