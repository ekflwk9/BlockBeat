using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUi : UiBase
{
    [SerializeField] private RankSystem rankSystem;
    [SerializeField] private GameObject connectionWindow;

    [SerializeField] private TMP_Text[] user;
    [SerializeField] private TMP_Text[] point;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<RankUi>();
        GetRankSystem();
        FindWindow();
        FindText();
    }

    private void GetRankSystem()
    {
        rankSystem = this.RequireComponent<RankSystem>();
    }

    private void FindWindow()
    {
        connectionWindow = this.TryFindChild("ConnectionWindow");
        connectionWindow?.SetActive(false);

        var connectText = connectionWindow.transform.TryGetChildComponent<TMP_Text>();
        if (connectText) connectText.text = "No network connection.\nPlease connect to a network\nand restart the app.";
    }

    private void FindText()
    {
        var tempUser = new List<TMP_Text>();
        var tempScore = new List<TMP_Text>();

        for (int i = 0; i < RankSystem.maxCount; i++)
        {
            var parent = this.TryFindChild($"User {i + 1}");

            if (!parent)
            {
                Service.Log($"User {i + 1}는 없음");
                continue;
            }

            var userText = parent.transform.TryGetChildComponent<TMP_Text>("User");
            userText.text = string.Empty;
            tempUser.Add(userText);

            var scoreText = parent.transform.TryGetChildComponent<TMP_Text>("Point");
            scoreText.text = string.Empty;
            tempScore.Add(scoreText);

            if (RankSystem.maxCount == tempUser.Count) break;
        }

        user = tempUser.ToArray();
        point = tempScore.ToArray();
    }
#endif

    private void Start()
    {
        if (FirebaseManager.connect) GetRanker();
        else connectionWindow.SetActive(true);

        UiManager.Get<FadeUi>().FadeOut(0.4f);
    }

    private void GetRanker()
    {
        var ranker = rankSystem.ranker;

        for (int i = 0; i < user.Length; i++)
        {
            user[i].text = ranker[i].Key;
            point[i].text = $"{ranker[i].Value} pts";
        }
    }
}
