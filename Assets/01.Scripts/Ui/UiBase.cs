using UnityEngine;
using UnityEngine.UI;

public abstract class UiBase : MonoBehaviour
{
    protected virtual void Awake()
    {
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
    /// UI 활성화
    /// </summary>
    public virtual void On() => this.gameObject.SetActive(true);

    /// <summary>
    /// UI 비활성화
    /// </summary>
    public virtual void Off() => this.gameObject.SetActive(false);
}
