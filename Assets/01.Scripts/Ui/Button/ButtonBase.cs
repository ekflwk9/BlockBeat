using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected RectTransform touch;

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        touch = this.TryGetChildComponent<RectTransform>("Touch");
        if (touch) touch.gameObject.SetActive(false);
    }
#endif

    private void OnDisable() => touch?.gameObject.SetActive(false);

    public virtual void OnPointerDown(PointerEventData eventData) => touch?.gameObject.SetActive(true);

    public void OnPointerUp(PointerEventData eventData) => touch?.gameObject.SetActive(false);
}
