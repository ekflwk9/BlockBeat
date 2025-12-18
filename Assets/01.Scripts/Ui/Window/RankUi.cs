using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUi : UiBase
{
    private const int rankMaxCount = 7;

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text[] user = new TMP_Text[rankMaxCount];
    [SerializeField] private TMP_Text[] point = new TMP_Text[rankMaxCount];

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

            if (rankMaxCount == tempUser.Count) break;
        }

        user = tempUser.ToArray();
        point = tempScore.ToArray();
    }
#endif

    private void Awake()
    {
        SetUser();
        SetScore();

        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void SetUser()
    {
        for (int i = 0; i < FirebaseManager.user.Length; i++)
        {
            user[i].text = FirebaseManager.user[i];
        }
    }

    private void SetScore()
    {
        for (int i = 0; i < FirebaseManager.point.Length; i++)
        {
            var newText = $"{FirebaseManager.point[i]} pts";
            point[i].text = newText;
        }
    }
}
