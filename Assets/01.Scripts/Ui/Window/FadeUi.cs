using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeUi : UiBase
{
    [Header("콤보 성공시 색깔"), SerializeField] private Color combo;
    [Header("게임 오버시 색깔"), SerializeField] private Color gameOver;

    [Space(20f)]
    [SerializeField] private Image uiFade;
    [SerializeField] private SpriteRenderer gameFade;

    private Action fadeFunc;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<FadeUi>();
        gameFade = this.TryGetChildComponent<SpriteRenderer>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 100;

        uiFade = this.TryGetChildComponent<Image>();

        if (uiFade)
        {
            uiFade.color = Color.black;
            uiFade.color = Color.clear;
        }
    }
#endif

    private void Start()
    {
        Init();
        InitGameFade();
    }

    private void Init()
    {
        this.transform.SetParent(UiManager.Instance);
    }

    private void InitGameFade()
    {
        var screenPos = new Vector3(0.5f, 0.5f);
        var fadePos = CamController.Instatnce.cam.ViewportToWorldPoint(screenPos);
        fadePos.z = 0f;

        gameFade.transform.position = fadePos;
        gameFade.transform.localScale = new Vector3(Screen.width, Screen.height);
        gameFade.color = Color.clear;
    }

    /// <summary>
    /// UI 페이드 인
    /// </summary>
    /// <param name="_timer"></param>
    /// <param name="_func"></param>
    public void FadeIn(float _timer = 1f, Action _func = null)
    {
        uiFade.raycastTarget = true;
        uiFade.color = Color.clear;
        fadeFunc = _func;

        var tween = uiFade.DOFade(1f, _timer);
        if (_func != null) tween.OnComplete(EndFadeIn);
    }

    private void EndFadeIn()
    {
        fadeFunc?.Invoke();
        fadeFunc = null;
    }

    /// <summary>
    /// UI 페이드 아웃
    /// </summary>
    /// <param name="_timer"></param>
    public void FadeOut(float _timer = 1f)
    {
        uiFade.raycastTarget = false;
        uiFade.color = Color.black;
        uiFade.DOFade(0f, _timer);
    }

    /// <summary>
    /// 게임속 페이드 아웃
    /// </summary>
    /// <param name="_timer"></param>
    public void ComboFadeOut()
    {
        gameFade.DOKill();

        gameFade.color = combo;
        gameFade.DOFade(0f, 0.5f);
    }

    /// <summary>
    /// 게임속 페이드 아웃
    /// </summary>
    /// <param name="_timer"></param>
    public void GameOverFadeOut()
    {
        gameFade.DOKill();

        gameFade.color = gameOver;
        gameFade.DOFade(0f, 0.5f);
    }
}
