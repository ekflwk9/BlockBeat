using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUi : UiBase
{
    [SerializeField] private RankSystem rank;
    [SerializeField] private Image line;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<MainUi>();
        line = this.TryGetChildComponent<Image>("StartLine");
        rank = this.RequireComponent<RankSystem>();
    }
#endif

    private void Start()
    {
        InitStartLine();
        InitCoinText();
        InitMainSound();
        InitUi();
    }

    private void OnDestroy()
    {
        line?.DOKill();
    }

    private void InitCoinText()
    {
        var coinText = this.TryGetChildComponent<TMP_Text>("CoinText");
        if (coinText) coinText.text = Json.GetCoin().ToString("N0");
    }

    private void InitStartLine()
    {
        var tween = line.DOFade(0f, 0.5f);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
    }

    private void InitUi()
    {
        UiManager.Get<FadeUi>().FadeOut();
        if (rank.newRecord) UiManager.On<AddPopupUi>();
    }

    private void InitMainSound()
    {
        SoundManager.OnMusic(SoundManager.SoundName.MainMusic);
    }
}
