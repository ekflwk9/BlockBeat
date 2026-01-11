using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private TMP_Text evade;
    [SerializeField] private TMP_Text evadeTitle;

    [SerializeField] private Image gameOverFade;
    private Coroutine coroutine;

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

        evadeTitle = this.TryGetChildComponent<TMP_Text>("EvadeTitle");
        evade = this.TryGetChildComponent<TMP_Text>("Evade");

        gameOverFade = this.TryGetChildComponent<Image>("GameOverFade");
    }
#endif

    private void Start()
    {
        ShowFade();
        SetComment();
        SetPlayPoint();
        SetMaxPoint();
        SetEvade();
        SetTimer();
        OnMusic();
    }

    private void OnDestroy()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    private void ShowFade()
    {
        UiManager.Get<FadeUi>().FadeOut(0.3f);
        coroutine = StartCoroutine(OnGameOverFade());
    }

    private IEnumerator OnGameOverFade()
    {
        var loop = 4;
        var speed = 0.07f;

        for (int i = 0; i < loop; i++)
        {
            gameOverFade.color = Color.white;
            yield return CoroutineManager.Wait(speed);
            gameOverFade.color = Color.clear;
            yield return CoroutineManager.Wait(speed);
        }
    }

    private void OnMusic()
    {
        if (Json.GetAdvertPass() < AdvertisementSystem.maxPassCount)
        {
            SoundManager.OnMusic(SoundManager.SoundName.GameOver, false);
        }
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

        var fontSize = maxPoint.fontSize * 0.8f;

#if !UNITY_EDITOR
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
#else
        var playPoint = Json.GetPlayPoint();
        var playMaxPoint = Json.GetPlayMaxPoint();

        var newText = $"<size={fontSize}>New !</size>";
        var newRecord = (playMaxPoint != 0 && playMaxPoint == playPoint) ? $"<color=#00FF00>{newText}</color>" : string.Empty;

        maxPoint.text = $"{newRecord}    {playMaxPoint}";
#endif
    }

    private void SetEvade()
    {
        evadeTitle.text = "Most Evade";
        evade.text = Json.GetEvade().ToString();
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
