using UnityEngine;

public abstract class UiBase : MonoBehaviour
{
    protected void SetName<T>() where T : UiBase
    {
        this.name = typeof(T).Name;
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
