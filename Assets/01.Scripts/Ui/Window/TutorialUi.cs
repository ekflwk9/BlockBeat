using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialUi : UiBase, IPointerClickHandler
{
    [SerializeField] private CanvasGroup group;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<TutorialUi>();

        group = this.RequireComponent<CanvasGroup>();

        var infoT = this.TryGetChildComponent<TMP_Text>("Info");
        if (infoT) infoT.text = "Press the thornless direction \nto remove the block!";
    }
#endif

    public override void On()
    {
        GlobalVolumeManager.SetVignette(0.35f, 0f);
        base.On();
    }

    public override void Off()
    {
        OffAnimation();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OffAnimation();
    }

    private void OffAnimation()
    {
        if (group.alpha != 1) return;

        group.DOKill();
        GlobalVolumeManager.SetVignette(0f, 0.2f);

        var tween = group.DOFade(0f, 0.2f);
        tween.OnComplete(EndFade);
    }

    private void EndFade()
    {
        this.gameObject.SetActive(false);
    }
}
