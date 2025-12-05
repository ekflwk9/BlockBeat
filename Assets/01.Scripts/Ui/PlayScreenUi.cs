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
        if (eventData.position.x < this.transform.position.x) BlockController.Instance?.MovePlayer(true);
        else BlockController.Instance?.MovePlayer(false);
    }
}
