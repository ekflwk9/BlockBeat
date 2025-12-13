using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreUi : UiBase
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup group;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ScoreUi>();
        group = this.RequireComponent<CanvasGroup>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 10;

        text = this.TryGetChildComponent<TMP_Text>();
        if (text) text.text = "0";
    }
#endif

    public override void On()
    {
        base.On();

        group.alpha = 0f;
        group.DOFade(1f, 0.5f);
    }

    /// <summary>
    /// 점수 UI 업데이트
    /// </summary>
    public void UpScore()
    {
        text.text = Json.GetPlayScore().ToString();
    }
}
