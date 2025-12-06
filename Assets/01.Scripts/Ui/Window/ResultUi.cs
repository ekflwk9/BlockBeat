using TMPro;
using UnityEngine;

public class ResultUi : UiBase
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text score;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ResultUi>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 20;

        title = this.TryGetChildComponent<TMP_Text>("Title");
        score = this.TryGetChildComponent<TMP_Text>("Score");
    }
#endif

    private void Awake()
    {
        score.text = Json.GetPlayerScore().ToString();
    }
}
