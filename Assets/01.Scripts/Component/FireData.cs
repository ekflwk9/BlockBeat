using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FireData : MonoBehaviour
{
    private DatabaseReference data;
    private FirebaseAuth auth;

    private Dictionary<string, int> score = new(capacity: 10);

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(Login);
    }

    [Button]
    private void Forwarding()
    {
        if (data == null) Service.Log("아직 로그인 안됨");

        var userName = "user_01";
        var child = $"users/{auth.CurrentUser.UserId}/{userName}/Score";
        data.Child(child).SetValueAsync(100);
    }

    private async void Login(Task<DependencyStatus> _task)
    {
        if (_task.Result != DependencyStatus.Available) return;
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null) await auth.SignInAnonymouslyAsync();
        data = FirebaseDatabase.DefaultInstance.RootReference;
    }
}
