using System.Collections;
using UnityEngine;

public class IntroUi : UiBase
{
    private const float timer = 1.5f;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ResultUi>();
    }
#endif

    private void Awake()
    {
        InitGlobalVolume();
        InitFrame();
        InitTimer();
    }

    private void InitGlobalVolume()
    {
        GlobalVolumeManager.SetVignette(0f, 0f);
    }

    private void InitFrame()
    {
        Application.targetFrameRate = 60;
    }

    private void InitTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return CoroutineManager.Wait(timer);
        UiManager.Get<FadeUi>().FadeIn(EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Main);
    }
}
