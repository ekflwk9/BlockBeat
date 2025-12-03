using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayScreenUi : UiBase, IPointerClickHandler
{
#if UNITY_EDITOR
    private void Reset()
    {
        SetName<PlayScreenUi>();

        var image = this.TryGetChildComponent<Image>();
        if(image) image.color = Color.clear;
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.position.x < this.transform.position.x) BlockManager.MovePlayer(-1);
        else BlockManager.MovePlayer(1);
    }
}
