using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialUi : UiBase, IPointerClickHandler
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image leftLine;
    [SerializeField] private Image rightLine;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<TutorialUi>();

        leftLine = this.TryGetChildComponent<Image>("LeftLine");
        rightLine = this.TryGetChildComponent<Image>("RightLine");
        group = this.RequireComponent<CanvasGroup>();

        var infoT = this.TryGetChildComponent<TMP_Text>("Info");
        if (infoT) infoT.text = "Press the thornless direction \nto remove the block!";
    }
#endif

    public override void On()
    {
        GlobalVolumeManager.SetVignette(0.35f, 0f);
        base.On();

        OnButtontLine(leftLine);
        OnButtontLine(rightLine);
    }

    public override void Off()
    {
        OffAnimation();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OffAnimation();
    }

    private void OnButtontLine(Image _image)
    {
        var tween = _image.DOFade(0f, 0.5f);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
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
        leftLine.DOKill();
        rightLine.DOKill();
        this.gameObject.SetActive(false);
    }
}
