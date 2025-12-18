using System;
using UnityEngine;

public class RankSystem : MonoBehaviour
{
    public const int maxCount = 7;

    [field: SerializeField] public int[] point { get; private set; }
    [field: SerializeField] public string[] user { get; private set; }

#if UNITY_EDITOR
    private void Reset()
    {
        this.name = typeof(RankSystem).Name;
    }
#endif

    private void Awake()
    {
        Json.Save();

        if (FirebaseManager.user != null)
        {
            Service.Log("FirebaseManager 로드 안됨");
            return;
        }

        Sort();
        Rank();
    }

    private void Sort()
    {
        user = FirebaseManager.user;
        point = FirebaseManager.point;

        Array.Sort(point, user);
    }

    private void Rank()
    {
        for (int i = maxCount - 1; 0 <= i; i--)
        {
            if (string.Equals(user[i], Json.GetName()))
            {
                if (point[i] < Json.GetPlayPoint()) FirebaseManager.Add();
                return;
            }
        }

        for (int i = maxCount - 1; 0 <= i; i--)
        {
            if (point[i] < Json.GetPlayPoint())
            {
                FirebaseManager.Add();
            }
        }
    }
}
