using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainUi : UiBase
{
    [SerializeField] private Image line;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<MainUi>();
        line = this.TryGetChildComponent<Image>("StartLine");
    }
#endif

    private void Start()
    {
        InitStartLine();
        UiManager.Get<FadeUi>().FadeOut(0.4f);
        GlobalVolumeManager.SetVignette(0f, 0f);
    }

    private void OnDestroy()
    {
        line?.DOKill();
    }

    private void InitStartLine()
    {
        var tween = line.DOFade(0f, 0.5f);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
    }
}
