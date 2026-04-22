using TMPro;
using UnityEngine;

public class MainUi : UiBase
{
    [SerializeField] private RankSystem rank;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<MainUi>();
        rank = this.RequireComponent<RankSystem>();
    }
#endif

    private void Start()
    {
        InitCoinText();
        InitMainSound();
        InitUi();
    }

    private void InitCoinText()
    {
        var coinText = this.TryGetChildComponent<TMP_Text>("CoinText");
        if (coinText) coinText.text = Json.GetCoin().ToString("N0");
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
