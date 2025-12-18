using UnityEngine;

public class RankSystem : MonoBehaviour
{
#if UNITY_EDITOR
    private void Reset()
    {
        this.name = typeof(RankSystem).Name;
        this.transform.position = Vector3.zero;
    }
#endif

    private void Awake()
    {
        Json.Save();

        if (FirebaseManager.user != null) Rank();
        else Service.Log("FirebaseManager 로드 안됨");
    }

    private void Rank()
    {
        var maxRank = FirebaseManager.maxRankCount - 1;
        var user = FirebaseManager.user;
        var point = FirebaseManager.point;

        for (int i = maxRank; 0 <= i; i--)
        {
            if (string.Equals(user[i], Json.GetName()))
            {
                if (point[i] < Json.GetPlayPoint())
                {
                    FirebaseManager.Add();
                    FirebaseManager.Sort(i);
                }

                return;
            }
        }

        for (int i = maxRank; 0 <= i; i--)
        {
            if (point[i] < Json.GetPlayPoint())
            {
                FirebaseManager.Add();
                FirebaseManager.Sort(i);
            }
        }
    }
}
