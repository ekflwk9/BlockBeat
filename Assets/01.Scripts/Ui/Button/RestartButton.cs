using UnityEngine.EventSystems;

public class RestartButton : ButtonBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        SceneChangeManager.Add(EndLoad);
        UiManager.Get<FadeUi>().FadeIn(0.5f, EndFade);
    }
    private void EndFade()
    {
        SceneChangeManager.Change(SceneChangeManager.SceneName.Play);
    }

    private void EndLoad()
    {
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }
}
