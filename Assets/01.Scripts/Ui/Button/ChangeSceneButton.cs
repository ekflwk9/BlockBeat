using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSceneButton : ButtonBase
{
    [Header("이동할 씬"), SerializeField] private SceneChangeManager.SceneName sceneName;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        SceneChangeManager.Add(EndLoad);
        UiManager.Get<FadeUi>().FadeIn(0.5f, EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(sceneName);
    }

    private void EndLoad()
    {
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }
}
