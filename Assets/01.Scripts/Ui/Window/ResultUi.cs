using TMPro;
using UnityEngine;

public class ResultUi : UiBase
{
    [SerializeField] private TMP_Text commentTitle;
    [SerializeField] private LevelSystem level;

    [SerializeField] private TMP_Text pointTitle;
    [SerializeField] private TMP_Text point;

    [SerializeField] private TMP_Text maxPointTitle;
    [SerializeField] private TMP_Text maxPoint;

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
        level = this.RequireComponent<LevelSystem>();

        pointTitle = this.TryGetChildComponent<TMP_Text>("PointTitle");
        point = this.TryGetChildComponent<TMP_Text>("Point");

        timerTitle = this.TryGetChildComponent<TMP_Text>("TimerTitle");
        timer = this.TryGetChildComponent<TMP_Text>("Timer");

        maxPointTitle = this.TryGetChildComponent<TMP_Text>("MaxPointTitle");
        maxPoint = this.TryGetChildComponent<TMP_Text>("MaxPoint");

        comboTitle = this.TryGetChildComponent<TMP_Text>("ComboTitle");
        combo = this.TryGetChildComponent<TMP_Text>("Combo");
    }
#endif

    private void Start()
    {
        ShowFade(); 
        SetComment();
        SetPlayPoint();
        SetMaxPoint();
        SetCombo();
        SetTimer();
    }

    private void ShowFade()
    {
        UiManager.Get<FadeUi>().FadeOut(0.4f);
    }

    private void SetComment()
    {
        var index = level.GetLevel();

        if (index < 0) index = 0;
        else if (comment.Length <= index) index = comment.Length - 1;

        commentTitle.text = comment[index];
    }

    private void SetPlayPoint()
    {
        pointTitle.text = "Blocks Broken";
        point.text = Json.GetPlayPoint().ToString();
    }

    private void SetMaxPoint()
    {
        //$"<size={지정할 폰트 사이즈}>{출력할 string}</size>";
        //"<color=#FF0000>{출력할 string}</color>";

        maxPoint.color = Color.white;
        maxPointTitle.text = "Best";

        var fontSize = maxPoint.fontSize * 0.6f;

        if (FirebaseManager.connect)
        {
            var playPoint = Json.GetPlayPoint();
            var playMaxPoint = Json.GetPlayMaxPoint();


            var newText = $"<size={fontSize}>New !</size>";
            var newRecord = (playMaxPoint != 0 && playMaxPoint == playPoint) ? $"<color=#00FF00>{newText}</color>" : string.Empty;

            maxPoint.text = $"{newRecord}    {playMaxPoint}";
        }

        else
        {
            var newText = "No network connection.";
            var newSize = $"<size={fontSize}>{newText}</size>";

            maxPoint.color = Color.gray;
            maxPoint.text = newSize;
        }
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
