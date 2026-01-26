using TMPro;
using UnityEngine.EventSystems;

public class AddRankButton : ButtonBase
{
#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        var text = this.TryGetChildComponent<TMP_Text>();
        if (text) text.text = "OK";
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (UiManager.Get<AddPopupUi>().CanChangeName())
        {
            UiManager.Off<AddPopupUi>();
            UiManager.Get<FadeUi>().FadeIn(ChangeRankSene);
        }
    }

    private void ChangeRankSene()
    {
        var newName = UiManager.Get<AddPopupUi>().InputName();
        Json.SetName(newName);
        Json.Save();

        FirebaseManager.AddRank();
        SceneChangeManager.Change(SceneChangeManager.SceneName.Rank);
    }
}