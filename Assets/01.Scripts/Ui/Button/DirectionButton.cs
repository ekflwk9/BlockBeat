using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionButton : ButtonBase
{
    [SerializeField] private bool isLeft;

#if UNITY_EDITOR
    private void Reset()
    {
        var rect = this.transform as RectTransform;
        if (rect) isLeft = 0f < rect.anchorMin.x ? false : true;
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        GameManager.Instance?.Touch(isLeft);
    }
}
