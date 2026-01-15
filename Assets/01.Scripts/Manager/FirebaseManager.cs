using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class FirebaseManager
{
    public const string referenceName = "leaderboard";
    //데이터 항목
    public const string pointName = "point";
    public const string userName = "nickname";

    public static bool connect { get; private set; }
    public static Dictionary<string, int> value { get; private set; } = new();

#if !UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
#endif
    private static void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Login);
    }

    private static async void Login(Task<DependencyStatus> _task)
    {
        try
        {
            if (_task.Result != DependencyStatus.Available) return;
            var auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null) await auth.SignInAnonymouslyAsync();
            Load();
        }

        catch
        {
            connect = false;
        }
    }

    private static async void Load()
    {
        try
        {
            var root = FirebaseDatabase.DefaultInstance.GetReference(referenceName);
            var values = await root.GetValueAsync();

            connect = true;
            InitData(values);
        }

        catch
        {
            connect = false;
        }
    }

    private static void InitData(DataSnapshot _snap)
    {
        var enumerator = _snap.Children.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var currentUser = enumerator.Current; //현재 유저 레퍼런스

            var _nickName = currentUser.Child(userName).Value.ToString();
            var _point = System.Convert.ToInt32(currentUser.Child(pointName).Value);

            if (!value.ContainsKey(_nickName)) value.Add(_nickName, _point);
            else Service.Log($"{_nickName}은 이미 추가됨");
        }
    }

    /// <summary>
    /// 파이어 베이스 랭커 등록
    /// </summary>
    public static void AddRank()
    {
        if (!connect) return; 

        //내 데이터 경로
        var userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var myData = $"{referenceName}/{userID}";
        var userPath = $"{myData}/{userName}";
        var scorePath = $"{myData}/{pointName}";

        var playerNickName = Json.GetName();
        var playerPoint = Json.GetPlayMaxPoint();
        var defaultReference = FirebaseDatabase.DefaultInstance.RootReference;

        //해당 경로 파이어 베이스 데이터에 올림
        defaultReference.Child(userPath).SetValueAsync(playerNickName);
        defaultReference.Child(scorePath).SetValueAsync(playerPoint);

        //로컬 데이터 변경
        if (value.ContainsKey(playerNickName)) value[playerNickName] = playerPoint;
        else value.Add(playerNickName, playerPoint);
    }
}