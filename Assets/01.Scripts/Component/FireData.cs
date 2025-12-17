using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;

public class FireData : MonoBehaviour
{
    private DatabaseReference data;
    private Dictionary<string, int> score = new(capacity: 10);

    private void Start()
    {
        data = FirebaseDatabase.DefaultInstance.RootReference;
        Forwarding();
    }

    private void Forwarding()
    {
        var userName = "user_001";
        var child = $"users/{userName}/Score";

        data.Child(child).SetValueAsync(100);
    }
}
