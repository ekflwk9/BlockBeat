using TMPro;
using UnityEngine.EventSystems;

public class RankInfoButton : ButtonBase
{

#if UNITY_EDITOR
    protected override void Reset()
    {
        var text = this.TryGetChildComponent<TMP_Text>();
        if (text) text.text = "OK";
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!UiManager.Get<InfoUi>().SameName())
        {
            base.OnPointerDown(eventData);
            UiManager.Get<FadeUi>().FadeIn(0.3f, ChangeRankSene);
        }
    }

    private void ChangeRankSene()
    {
        var newName = UiManager.Get<InfoUi>().InputName();
        Json.SetName(newName);

        FirebaseManager.AddRank();
        SceneChangeManager.Change(SceneChangeManager.SceneName.Rank);
    }
}