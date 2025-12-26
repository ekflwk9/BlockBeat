using UnityEngine.EventSystems;

public class ShopSelectButton : ButtonBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UiManager.Get<ShopUi>().SetMainBlock();
    }
}
