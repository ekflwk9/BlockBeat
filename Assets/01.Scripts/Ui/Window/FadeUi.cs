using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeUi : UiBase
{
    [SerializeField] private Image image;
    private Action fadeFunc;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<FadeUi>();
        image = this.TryGetChildComponent<Image>();
    }
#endif

    /// <summary>
    /// 페이드 인
    /// </summary>
    /// <param name="_timer"></param>
    /// <param name="_func"></param>
    public void FadeIn(float _timer = 1f, Action _func = null)
    {
        fadeFunc = _func;
        var tween =  image.DOFade(1f, _timer);
        if (_func != null) tween.OnComplete(EndFadeIn);
    }

    private void EndFadeIn()
    {
        fadeFunc?.Invoke();
        fadeFunc = null;
    }

    /// <summary>
    /// 페이드 아웃
    /// </summary>
    /// <param name="_timer"></param>
    public void FadeOut(float _timer)
    {
        image.DOFade(0f, _timer);
    }
}
