using UnityEngine.EventSystems;

public class MainButton : ButtonBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        SceneChangeManager.Add(EndLoad);
        UiManager.Get<FadeUi>().FadeIn(0.5f, EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Start);
    }

    private void EndLoad()
    {
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }
}
