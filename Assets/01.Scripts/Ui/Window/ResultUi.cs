using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUi : UiBase
{
    [SerializeField] private TMP_Text record;
    [SerializeField] private TMP_Text score;

    [SerializeField] private TMP_Text maxScoreTitle;
    [SerializeField] private TMP_Text maxScore;

    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text timerTitle;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<ResultUi>();

        var canvas = this.TryGetComponent<Canvas>();
        if (canvas) canvas.sortingOrder = 20;

        record = this.TryGetChildComponent<TMP_Text>("Record");
        score = this.TryGetChildComponent<TMP_Text>("Score");

        timer = this.TryGetChildComponent<TMP_Text>("Timer");
        timerTitle = this.TryGetChildComponent<TMP_Text>("TimerTitle");

        maxScore = this.TryGetChildComponent<TMP_Text>("MaxScore");
        maxScoreTitle = this.TryGetChildComponent<TMP_Text>("MaxScoreTitle");
    }
#endif

    private void Awake()
    {
        SetResolution();
        SetUi();
        SetFont();
    }

    private void SetResolution()
    {
        var cavans = this.TryGetComponent<CanvasScaler>();
        cavans.referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    private void SetUi()
    {
        var playScore = Json.GetPlayScore();

        if (Json.GetPlayMaxScore() < playScore)
        {
            Json.PlayMaxScore(playScore);

            record.gameObject.SetActive(true);
            OnRecordAnimation();
        }
    }

    private void OnRecordAnimation()
    {
        var tween = record.DOFade(0.25f, 0.3f);
        tween.SetEase(Ease.OutSine);
        tween.SetLoops(-1, LoopType.Yoyo);
    }

    private void SetFont()
    {
        //$"<size={지정할 폰트 사이즈}>{출력할 string}</size>";
        var fontSzie = score.fontSize * 0.7f;
        score.text = $"{Json.GetPlayScore()}\n<size={fontSzie}>Blocks Broken</size>";

        maxScoreTitle.text = "Most Blocks Broken";
        maxScore.text = $"{Json.GetPlayMaxScore()}";

        var playTime = Json.GetPlayTime();
        var minutes = (int)(playTime / 60);
        var second = minutes < 1 ? playTime : playTime % 60;

        timerTitle.text = "Survival Time";

        if(minutes < 1) timer.text = $"{playTime.ToString("F2")}s";
        else timer.text = $"{minutes}m {second.ToString("F2")}s";
    }
}
