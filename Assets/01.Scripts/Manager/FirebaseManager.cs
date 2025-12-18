using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class FirebaseManager
{
    public const string referenceName = "leaderboard";
    public const string userName = "nickname";
    public const string pointName = "point";

    private static DatabaseReference data;
    private static FirebaseAuth auth;

    //
    public const int maxRankCount = 7; //랭킹 표시 제한
    public static int[] point { get; private set; }
    public static string[] user { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Login);
        Load();
    }

    private static async void Login(Task<DependencyStatus> _task)
    {
        if (_task.Result != DependencyStatus.Available) return;
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null) await auth.SignInAnonymouslyAsync();
        data = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private static async void Load()
    {
        var root = FirebaseDatabase.DefaultInstance.GetReference(referenceName); //해당 이름 레퍼런스 반환
        var values = await root.GetValueAsync();  //해당 레퍼런스의 자식들만 모두 반환 (자식의 자식까지 X)

        InitData(values);
    }

    /// <summary>
    /// 본인의 닉네임과 점수를 파이어베이스 랭크에 등록
    /// </summary>
    public static void Add()
    {
        var child = $"{referenceName}/{auth.CurrentUser.UserId}";
        var userPath = $"{child}/{userName}";
        var scorePath = $"{child}/{pointName}";

        data.Child(userPath).SetValueAsync(Json.GetName());          //플레이어 이름을 파이어 베이스에 올림
        data.Child(scorePath).SetValueAsync(Json.GetPlayMaxPoint()); //최고 점수 파이어 베이스에 올림
    }

    #region 커스텀
    private static void InitData(DataSnapshot _snap)
    {
        var enumerator = _snap.Children.GetEnumerator();
        var index = 0;

        user = new string[_snap.ChildrenCount];
        point = new int[_snap.ChildrenCount];

        while (enumerator.MoveNext())
        {
            var currentUser = enumerator.Current; //현재 유저 레퍼런스

            user[index] = currentUser.Child(userName).Value.ToString();
            point[index] = System.Convert.ToInt32(currentUser.Child(pointName).Value);

            index++;
        }

        Array.Sort(point, user);
    }

    /// <summary>
    /// 로컬 데이터에서만 새롭게 정렬
    /// </summary>
    /// <param name="_targetIndex"></param>
    public static void Sort(int _targetIndex)
    {
        for (int i = maxRankCount - 1; 0 <= i; i--)
        {
            if (_targetIndex == i) break;

            user[i] = user[i - 1];
            point[i] = point[i - 1];
        }

        user[_targetIndex] = Json.GetName();
        point[_targetIndex] = Json.GetPlayPoint();
    }
    #endregion
}