using TMPro;
using UnityEngine;

public class ResultUi : UiBase
{
    [SerializeField] private TMP_Text commentTitle;
    [SerializeField] private GameLevel level;

    [SerializeField] private TMP_Text scoreTitle;
    [SerializeField] private TMP_Text score;

    [SerializeField] private TMP_Text maxScoreTitle;
    [SerializeField] private TMP_Text maxScore;

    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text timerTitle;

    [SerializeField] private TMP_Text combo;
    [SerializeField] private TMP_Text comboTitle;

    private string[] comment =
    {
        "You still have a long way to go.",
        "You've got some skills",
        "Are you a pro?",
    };

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ResultUi>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 20;

        commentTitle = this.TryGetChildComponent<TMP_Text>("CommentTitle");
        level = this.RequireComponent<GameLevel>();

        scoreTitle = this.TryGetChildComponent<TMP_Text>("ScoreTitle");
        score = this.TryGetChildComponent<TMP_Text>("Score");

        timerTitle = this.TryGetChildComponent<TMP_Text>("TimerTitle");
        timer = this.TryGetChildComponent<TMP_Text>("Timer");

        maxScoreTitle = this.TryGetChildComponent<TMP_Text>("MaxScoreTitle");
        maxScore = this.TryGetChildComponent<TMP_Text>("MaxScore");

        comboTitle = this.TryGetChildComponent<TMP_Text>("ComboTitle");
        combo = this.TryGetChildComponent<TMP_Text>("Combo");
    }
#endif

    private void Start()
    {
        SetComment();
        SetScore();
        SetCombo();
        SetTimer();
        ShowFade();
    }

    private void ShowFade()
    {
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void SetComment()
    {
        var index = level.GetLevel();

        if (index < 0) index = 0;
        else if (comment.Length <= index) index = comment.Length - 1;

        commentTitle.text = comment[index];
    }

    private void SetScore()
    {
        //$"<size={지정할 폰트 사이즈}>{출력할 string}</size>";
        //"<color=#FF0000>{출력할 string}</color>";

        var playScore = Json.GetPlayScore();
        var playMaxScore = Json.GetPlayMaxScore();

        var fontSize = maxScore.fontSize * 0.6f;
        var newText = $"<size={fontSize}>New !</size>";
        var newRecord = (playMaxScore != 0 && playMaxScore == playScore) ? $"<color=#00FF00>{newText}</color>" : string.Empty;

        scoreTitle.text = "Blocks Broken";
        score.text = playScore.ToString();

        maxScoreTitle.text = "Most Blocks Broken";
        maxScore.text = $"{newRecord}    {playMaxScore}";
    }

    private void SetCombo()
    {
        comboTitle.text = "Most Combo";
        combo.text = Json.GetCombo().ToString();
    }

    private void SetTimer()
    {
        var playTime = Json.GetPlayTime();
        var minutes = (int)(playTime / 60);
        var second = minutes < 1 ? playTime : playTime % 60;

        timerTitle.text = "Survival Time";

        if (minutes < 1) timer.text = $"{playTime.ToString("F2")}s";
        else timer.text = $"{minutes}m {second.ToString("F2")}s";
    }
}
