using UnityEngine.EventSystems;

public class ShopSelectButton : ButtonBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        var currentName = UiManager.Get<ShopUi>().currentBlockName;
        Json.SetMainBlock(currentName);
    }
}
