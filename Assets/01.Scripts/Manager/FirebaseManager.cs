using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;

public static class FirebaseManager
{
    private const string referenceName = "leaderboard";
    private const string userName = "nickname";
    private const string pointName = "point";

    public static int[] point { get; private set; }
    public static string[] user { get; private set; }

    private static DatabaseReference data;
    private static FirebaseAuth auth;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Login);
        LoadLeaderboard();
    }

    private static async void Login(Task<DependencyStatus> _task)
    {
        if (_task.Result != DependencyStatus.Available) return;
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null) await auth.SignInAnonymouslyAsync();
        data = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private static async void LoadLeaderboard()
    {
        var root = FirebaseDatabase.DefaultInstance.GetReference(referenceName); //해당 이름 레퍼런스 반환
        var values = await root.GetValueAsync();  //해당 레퍼런스의 자식들만 모두 반환 (자식의 자식까지 X)

        var enumerator = values.Children.GetEnumerator();
        var index = 0;

        user = new string[values.ChildrenCount];
        point = new int[values.ChildrenCount];

        while (enumerator.MoveNext())
        {
            var currentUser = enumerator.Current; //현재 유저 레퍼런스

            user[index] = currentUser.Child(userName).Value.ToString();
            point[index] = System.Convert.ToInt32(currentUser.Child(pointName).Value);

            index++;
        }

        RankSort();
    }

    private static void RankSort()
    {
        Array.Sort(point, user);
        var length = user.Length < 7 ? user.Length : 7;

        int[] topScore = new int[length];
        string[] topPlayer = new string[length];

        for (int i = 0; i < length; i++)
        {
            topPlayer[i] = user[i];
            topScore[i] = point[i];
        }

        user = topPlayer;
        point = topScore;
    }

    /// <summary>
    /// 본인의 닉네임과 점수를 파이어베이스 랭크에 등록
    /// </summary>
    public static void AddData()
    {
        var child = $"{referenceName}/{auth.CurrentUser.UserId}";
        var userPath = $"{child}/{userName}";
        var scorePath = $"{child}/{pointName}";

        data.Child(userPath).SetValueAsync(Json.GetName());          //플레이어 이름을 파이어 베이스에 올림
        data.Child(scorePath).SetValueAsync(Json.GetPlayMaxPoint()); //최고 점수 파이어 베이스에 올림
    }
}