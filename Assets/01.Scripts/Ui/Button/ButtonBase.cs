using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject touch;

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        touch = this.TryFindChild("Touch");

        if (touch)
        {
            var rect = touch.GetComponent<RectTransform>();
            var thisRect = this.GetComponent<RectTransform>();

            touch.transform.position = this.transform.position;
            rect.sizeDelta = thisRect.sizeDelta;
            touch.SetActive(false);
        }
    }
#endif

    protected virtual void OnDisable() => touch?.SetActive(false);

    public abstract void OnPointerClick(PointerEventData eventData);

    public virtual void OnPointerEnter(PointerEventData eventData) => touch?.SetActive(true);

    public virtual void OnPointerExit(PointerEventData eventData) => touch?.SetActive(false);
}
