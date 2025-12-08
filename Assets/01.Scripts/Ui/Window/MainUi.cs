using UnityEngine;

public class MainUi : UiBase
{
#if UNITY_EDITOR
    private void Reset()
    {
        SetName<MainUi>();
    }
#endif

    private void Start()
    {
        UiManager.Get<FadeUi>().FadeOut(0.5f);
        GlobalVolumeManager.SetVignette(0f, 0f);
    }
}
