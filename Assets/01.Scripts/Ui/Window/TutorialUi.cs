using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutorialUi : UiBase
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TMP_Text info;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<TutorialUi>();

        info = this.TryGetChildComponent<TMP_Text>();
        group = this.RequireComponent<CanvasGroup>();
    }
#endif

    public override void On()
    {
        GlobalVolumeManager.SetVignette(0.35f, 0f);

        var tween = info.DOFade(0.2f, 0.4f);
        tween.SetEase(Ease.OutSine);
        tween.SetLoops(-1, LoopType.Yoyo);

        base.On();
    }

    public override void Off()
    {
        info.DOKill();
        GlobalVolumeManager.SetVignette(0f, 0.2f);

        var tween = group.DOFade(0f, 0.2f);
        tween.OnComplete(EndFade);
    }

    private void EndFade()
    {
        this.gameObject.SetActive(false);
    }
}
