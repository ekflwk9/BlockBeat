using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUi : UiBase
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text[] user = new TMP_Text[RankSystem.maxCount];
    [SerializeField] private TMP_Text[] point = new TMP_Text[RankSystem.maxCount];

#if UNITY_EDITOR
    private void Reset()
    {
        SetName<RankUi>();

        title = this.TryGetChildComponent<TMP_Text>("Title");
        title.text = "Top 7 Players";

        FindText();
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

    private void Awake()
    {
        RankSort();
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void RankSort()
    {
        var topPlayer = FirebaseManager.user;
        var topPoint = FirebaseManager.point;

        Array.Sort(topPoint, topPlayer);

        for (int i = 0; i < RankSystem.maxCount; i++)
        {
            user[i].text = FirebaseManager.user[i];
            point[i].text = $"{FirebaseManager.point[i]} pts";
        }
    }
}
