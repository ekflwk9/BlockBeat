using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : ButtonBase
{
    [SerializeField] private UiBase window;

#if UNITY_EDITOR
    private void Reset()
    {
        window = this.transform.TryFindParent().TryGetComponent<UiBase>();
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        window.Off();
    }
}
