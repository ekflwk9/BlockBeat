using TMPro;
using UnityEngine;

public class CreditUi : UiBase
{
    [SerializeField] private TMP_Text info;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<CreditUi>();

        info = this.TryGetChildComponent<TMP_Text>("InfoText");
        SetText();
    }

    private void SetText()
    {
        var enter = "\n\n";
        var sound = "Sound - www.pixabay.com";
        var font = "PFStarders Font";
        var titleFont = "Stacked Font - Monkopus";

        info.text = $"{sound}{enter}{font}{enter}{titleFont}";
    }
#endif
}
