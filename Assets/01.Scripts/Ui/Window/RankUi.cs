using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUi : UiBase
{
    [SerializeField] private RankSystem rankSystem;

    [SerializeField] private TMP_Text title;
    [SerializeField] private GameObject connectionWindow;

    [SerializeField] private TMP_Text[] user;
    [SerializeField] private TMP_Text[] point;

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<RankUi>();
        GetRankSystem();
        FindWindow();
        SetTitle();
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
    }

    private void SetTitle()
    {
        title = this.TryGetChildComponent<TMP_Text>("Title");
        title.text = "Top 7 Players";
    }

    private void FindText()
    {
        var tempUser = new List<TMP_Text>();
        var tempScore = new List<TMP_Text>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            var parent = this.TryFindChild($"User {i + 1}");

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
        if (FirebaseManager.connect) RankSort();
        else connectionWindow.SetActive(true);

        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void RankSort()
    {
        var ranker = rankSystem.ranker;

        for (int i = 0; i < RankSystem.maxCount; i++)
        {
            user[i].text = ranker[i].Key;
            point[i].text = $"{ranker[i].Value} pts";
        }
    }
}
