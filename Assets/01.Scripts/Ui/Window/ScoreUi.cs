using TMPro;
using UnityEngine;

public class ScoreUi : UiBase
{
    [SerializeField] private TMP_Text text;
    private int score;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ScoreUi>();

        var canvas = this.TryGetComponent<Canvas>();
        canvas.sortingOrder = 10;

        text = this.TryGetChildComponent<TMP_Text>();
        if(text) text.text = "0";
    }
#endif

    /// <summary>
    /// 점수 UI 업데이트
    /// </summary>
    public void UpScore()
    {
        score++;
        text.text = score.ToString();
    }
}
