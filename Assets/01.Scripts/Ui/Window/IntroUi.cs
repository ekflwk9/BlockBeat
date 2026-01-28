using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IntroUi : UiBase
{
    private const float timer = 2f;
    private const float zoom = 1.1f;
    private const float startSize = 10f;
    private const float imageSize = 520f;

    [SerializeField] private Image logo;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<IntroUi>();
        logo = this.TryGetChildComponent<Image>("Logo");

        if (logo)
        {
            var size = imageSize - startSize;
            var newSize = new Vector2(size, size);

            logo.rectTransform.sizeDelta = newSize;
            logo.color = new Color(1f, 1f, 1f, 0f);
        }
    }
#endif

    private void OnDestroy()
    {
        logo.DOKill();
        logo.transform.DOKill();
    }

    protected override void Awake()
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
        UiManager.On<FadeUi>();

        var fade = logo.DOFade(1f, timer);
        var newScale = logo.transform.localScale * zoom;
        var tween = logo.transform.DOScale(newScale, timer);
        tween.OnComplete(EndAnimation);
    }

    private void EndAnimation()
    {
        UiManager.Get<FadeUi>().FadeIn(EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Main);
    }
}
