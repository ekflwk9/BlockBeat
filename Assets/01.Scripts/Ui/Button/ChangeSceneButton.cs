using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSceneButton : ButtonBase
{
    [Header("이동할 씬"), SerializeField] private SceneChangeManager.SceneName sceneName;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UiManager.Get<FadeUi>().FadeIn(0.4f, EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(sceneName);
    }
}
