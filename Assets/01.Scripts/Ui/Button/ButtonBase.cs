using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected RectTransform touch;

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        touch = this.TryGetChildComponent<RectTransform>("Touch");

        if (touch)
        {
            var thisRect = this.GetComponent<RectTransform>();

            touch.transform.position = this.transform.position;

            var size = thisRect.sizeDelta;
            size.x = size.x < 0 ? -size.x : size.x;

            touch.sizeDelta = size;
            touch.gameObject.SetActive(false);
        }
    }
#endif

    private void OnDisable() => touch?.gameObject.SetActive(false);

    public virtual void OnPointerDown(PointerEventData eventData) => touch?.gameObject.SetActive(true);

    public void OnPointerUp(PointerEventData eventData) => touch?.gameObject.SetActive(false);
}
