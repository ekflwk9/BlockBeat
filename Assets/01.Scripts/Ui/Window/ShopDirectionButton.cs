using UnityEngine;
using UnityEngine.EventSystems;

public class ShopDirectionButton : ButtonBase
{
    [SerializeField] private bool isLeft;

#if UNITY_EDITOR
    private void Reset()
    {
        var rect = this.transform as RectTransform;
        if (rect) isLeft = rect.anchorMax.x < 0.5f;
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UiManager.Get<ShopUi>().Move(isLeft);
    }
}
