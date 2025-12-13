using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSceneButton : ButtonBase
{
    [Header("이동할 씬"), SerializeField] private SceneChangeManager.SceneName sceneName;
    [Header("양쪽에 위치한 버튼인가?"), SerializeField] private bool isSideButton;

    private void Awake()
    {
        if (!isSideButton) return;

        var rect = this.TryGetComponent<RectTransform>();
        var newSize = rect.sizeDelta;

        newSize.x = Screen.width * 0.5f;
        rect.sizeDelta = newSize;
        touch.sizeDelta = newSize;

        var newPos = rect.anchoredPosition;
        newPos.x = newPos.x < 0f ? newSize.x * -0.5f : newSize.x * 0.5f;
        rect.anchoredPosition = newPos;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UiManager.Get<FadeUi>().FadeIn(0.5f, EndFade);
    }

    private void EndFade()
    {
        SceneChangeManager.Change(sceneName);
    }
}
