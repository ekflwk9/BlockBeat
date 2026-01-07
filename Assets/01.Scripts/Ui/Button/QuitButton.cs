using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : ButtonBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Application.Quit();
    }
}
