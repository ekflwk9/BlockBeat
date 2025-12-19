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

    public static bool connect { get; private set; }
    private static FirebaseAuth auth;
    private static DatabaseReference userReference;

    //
    public const string pointName = "point";
    public const string userName = "nickname";
    public const int maxRankCount = 7; //랭킹 표시 제한
    public static int[] point { get; private set; }
    public static string[] nickName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Login);
    }

    private static async void Login(Task<DependencyStatus> _task)
    {
        if (_task.Result != DependencyStatus.Available) return;
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null) await auth.SignInAnonymouslyAsync();
        userReference = FirebaseDatabase.DefaultInstance.RootReference;

        Load();
    }

    private static async void Load()
    {
        try
        {
            var root = FirebaseDatabase.DefaultInstance.GetReference(referenceName);
            var values = await root.GetValueAsync();

            connect = true;
            InitData(values);

            //추가
            DataSort();
        }

        catch
        {
            connect = false;
        }
    }

    /// <summary>
    /// 본인의 닉네임과 점수를 파이어베이스 랭크에 등록
    /// </summary>
    public static void Add()
    {
        if (!connect) return;

        var child = $"{referenceName}/{auth.CurrentUser.UserId}";
        var userPath = $"{child}/{userName}";
        var scorePath = $"{child}/{pointName}";

        userReference.Child(userPath).SetValueAsync(Json.GetName());          //플레이어 이름을 파이어 베이스에 올림
        userReference.Child(scorePath).SetValueAsync(Json.GetPlayMaxPoint()); //최고 점수 파이어 베이스에 올림
    }

    private static void InitData(DataSnapshot _snap)
    {
        nickName = new string[_snap.ChildrenCount];
        point = new int[_snap.ChildrenCount];

        var index = 0;
        var enumerator = _snap.Children.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var currentUser = enumerator.Current; //현재 유저 레퍼런스

            nickName[index] = currentUser.Child(userName).Value.ToString();
            point[index] = System.Convert.ToInt32(currentUser.Child(pointName).Value);

            index++;
        }
    }

    private static void DataSort()
    {
        Array.Sort(point, nickName);
        Array.Reverse(nickName);
        Array.Reverse(point);

        var maxLength = point.Length < maxRankCount ? point.Length : maxRankCount;
        var tempPoint = new int[maxLength];
        var tempUser = new string[maxLength];

        Array.Copy(nickName, tempUser, maxLength);
        Array.Copy(point, tempPoint, maxLength);

        nickName = tempUser;
        point = tempPoint;
    }

    /// <summary>
    /// 데이터 정렬
    /// </summary>
    /// <param name="_targetIndex"></param>
    public static void Sort(int _targetIndex)
    {
        for (int i = nickName.Length - 1; 0 <= i; i--)
        {
            if (_targetIndex == i)
            {
                //로컬에 등록
                nickName[i] = Json.GetName();
                point[i] = Json.GetPlayPoint();
                break;
            }

            else
            {
                //한칸씩 아래로 밀기
                nickName[i] = nickName[i - 1];
                point[i] = point[i - 1];
            }
        }
    }
}