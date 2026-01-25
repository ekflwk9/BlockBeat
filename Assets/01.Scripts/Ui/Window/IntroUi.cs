using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroUi : UiBase
{
    private const float timer = 1.5f;
    [SerializeField] private Image logo;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<IntroUi>();
        logo = this.TryGetChildComponent<Image>("Logo");
    }
#endif

    private void Awake()
    {
        InitGlobalVolume();
        InitFrame();
        InitLogo();
    }

    private void InitGlobalVolume()
    {
        GlobalVolumeManager.SetVignette(0f, 0f);
    }

    private void InitFrame()
    {
        Application.targetFrameRate = 60;
    }

    private void InitLogo()
    {
        var nextScale = logo.rectTransform.localScale * 1.1f;
        logo.rectTransform.DOScale(nextScale, timer);

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return CoroutineManager.Wait(timer);

        logo.DOKill();
        UiManager.Get<FadeUi>().FadeIn(EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Main);
    }
}
