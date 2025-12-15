using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionButton : ButtonBase
{
    [SerializeField] private bool isLeft;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        var rect = this.TryGetComponent<RectTransform>();
        if (rect) isLeft = 0f < rect.anchorMin.x ? false : true;
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        GameManager.Instance?.Touch(isLeft);
    }
}
