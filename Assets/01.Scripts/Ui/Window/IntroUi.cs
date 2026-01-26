using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroUi : UiBase
{
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
        OnAnimation();
    }

    private void InitGlobalVolume()
    {
        GlobalVolumeManager.SetVignette(0f, 0f);
    }

    private void InitFrame()
    {
        Application.targetFrameRate = 60;
    }

    private void OnAnimation()
    {
        SoundManager.OnEffect(SoundManager.SoundName.Logo);

        var newScale = logo.transform.localScale * 1.1f;
        var tween = logo.transform.DOScale(newScale, 0.2f);
        tween.OnComplete(OnOriginScale);
    }

    private void OnOriginScale()
    {
        var tween = logo.transform.DOScale(Vector3.one, 0.2f);
        tween.OnComplete(EndAnimation);
    }

    private void EndAnimation()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return CoroutineManager.Wait(0.7f);
        UiManager.Get<FadeUi>().FadeIn(EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Main);
    }
}
