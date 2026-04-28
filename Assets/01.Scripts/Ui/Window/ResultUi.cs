using System.Collections;
using TMPro;
using UnityEngine;

public class ResultUi : UiBase
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text commentTitle;

    [SerializeField] private TMP_Text pointTitle;
    [SerializeField] private TMP_Text point;

    [SerializeField] private TMP_Text maxPointTitle;
    [SerializeField] private TMP_Text maxPoint;

    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text timerTitle;

    [SerializeField] private TMP_Text evade;
    [SerializeField] private TMP_Text evadeTitle;

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

        title = this.TryGetChildComponent<TMP_Text>("Title");
        commentTitle = this.TryGetChildComponent<TMP_Text>("CommentTitle");

        pointTitle = this.TryGetChildComponent<TMP_Text>("PointTitle");
        point = this.TryGetChildComponent<TMP_Text>("Point");

        timerTitle = this.TryGetChildComponent<TMP_Text>("TimerTitle");
        timer = this.TryGetChildComponent<TMP_Text>("Timer");

        maxPointTitle = this.TryGetChildComponent<TMP_Text>("MaxPointTitle");
        maxPoint = this.TryGetChildComponent<TMP_Text>("MaxPoint");

        evadeTitle = this.TryGetChildComponent<TMP_Text>("EvadeTitle");
        evade = this.TryGetChildComponent<TMP_Text>("Evade");
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

#if UNITY_ANDROID || UNITY_IOS
        AdvertisementSystem.ShowBanner(false);
#endif
    }

    private void ShowFade()
    {
        UiManager.Get<FadeUi>().FadeOut();
        coroutine = StartCoroutine(OnGameOverFade());
    }

    private IEnumerator OnGameOverFade()
    {
        var loop = 4;
        var speed = 0.07f;

        for (int i = 0; i < loop; i++)
        {
            title.color = Color.black;
            yield return CoroutineManager.Wait(speed);
            title.color = Color.white;
            yield return CoroutineManager.Wait(speed);
        }
    }

    private void OnMusic()
    {
#if UNITY_ANDROID || UNITY_IOS
        var passCount = Json.PlayerData().advertPassCount;

        if (AdvertisementSystem.maxPassCount < passCount)
        {
            Json.SetAdvertPass(0);
            AdvertisementSystem.ShowVideo();
        }

        else
        {
            AdvertisementSystem.ShowBanner(true);
            SoundManager.OnMusic(SoundManager.SoundName.GameOver, false);

            if (50 < Json.GetPlayPoint()) Json.SetAdvertPass(passCount + 1);
        }
#endif
    }

    private void SetComment()
    {
        //if (index < 0) index = 0;
        //else if (comment.Length <= index) index = comment.Length - 1;

        //commentTitle.text = comment[index];

        commentTitle.text = "Test";
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
