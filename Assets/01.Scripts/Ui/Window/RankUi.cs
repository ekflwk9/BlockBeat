using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUi : UiBase
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text[] user = new TMP_Text[FirebaseManager.maxRankCount];
    [SerializeField] private TMP_Text[] point = new TMP_Text[FirebaseManager.maxRankCount];

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

            if (FirebaseManager.maxRankCount == tempUser.Count) break;
        }

        user = tempUser.ToArray();
        point = tempScore.ToArray();
    }
#endif

    private void Start()
    {
        if (FirebaseManager.connect) RankSort();
        else NotConnected();

        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }

    private void NotConnected()
    {

    }

    private void RankSort()
    {
        for (int i = 0; i < FirebaseManager.nickName.Length; i++)
        {
            user[i].text = FirebaseManager.nickName[i];
            point[i].text = $"{FirebaseManager.point[i]} pts";
        }
    }
}
