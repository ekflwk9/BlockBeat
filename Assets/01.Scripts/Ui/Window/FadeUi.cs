using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeUi : UiBase
{
    [SerializeField] private Image image;
    private Action fadeFunc;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<FadeUi>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 100;

        image = this.TryGetChildComponent<Image>();

        if (image)
        {
            image.color = Color.black;
            image.color = Color.clear;
        }
    }
#endif

    private void Start()
    {
        this.transform.SetParent(UiManager.Instance);
    }

    /// <summary>
    /// 페이드 인
    /// </summary>
    /// <param name="_timer"></param>
    /// <param name="_func"></param>
    public void FadeIn(float _timer = 1f, Action _func = null)
    {
        image.raycastTarget = true;
        image.color = Color.clear;
        fadeFunc = _func;

        var tween = image.DOFade(1f, _timer);
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
    public void FadeOut(float _timer = 1f)
    {
        image.raycastTarget = false;
        image.color = Color.black;
        image.DOFade(0f, _timer);
    }
}
