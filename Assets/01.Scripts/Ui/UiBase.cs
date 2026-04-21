using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class UiBase : MonoBehaviour
{
    public const float PopEffectSpeed = 0.15f;
    private float originButtonScale;

    protected virtual void Awake()
    {
        originButtonScale = this.transform.localScale.x;
        SetResolution();
    }

    protected void SetName<T>() where T : UiBase
    {
        this.name = typeof(T).Name;
    }

    /// <summary>
    /// 캔버스 해상도 설정
    /// </summary>
    protected void SetResolution()
    {
        var canvas = this.TryGetComponent<CanvasScaler>();
        var rect = this.TryGetComponent<RectTransform>();
        if (!canvas || !rect) return;

        float baseRatio = 720f / 1280f;
        var screen = rect.sizeDelta;
        var current = screen.x / screen.y;

        if (current > baseRatio) canvas.matchWidthOrHeight = 1f;
        else canvas.matchWidthOrHeight = 0f;
    }

    /// <summary>
    /// 커지며 등장 효과
    /// </summary>
    public void PopInEffect()
    {
        this.transform.DOKill();
        this.transform.localScale = Vector3.zero;

        var tween = this.transform.DOScale(originButtonScale * 1.25f, PopEffectSpeed);
        tween.OnComplete(PopIn);
    }

    private void PopIn() => this.transform.DOScale(originButtonScale, PopEffectSpeed);

    /// <summary>
    /// 작아지며 퇴장
    /// </summary>
    public void PopOutEffect()
    {
        this.transform.DOKill();

        var tween = this.transform.DOScale(originButtonScale * 1.25f, PopEffectSpeed);
        tween.OnComplete(PopOut);
    }

    private void PopOut() => this.transform.DOScale(0f, PopEffectSpeed);

    /// <summary>
    /// UI 활성화
    /// </summary>
    public virtual void On() => this.gameObject.SetActive(true);

    /// <summary>
    /// UI 비활성화
    /// </summary>
    public virtual void Off() => this.gameObject.SetActive(false);
}
