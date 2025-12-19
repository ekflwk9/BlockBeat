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
        if (FirebaseManager.connect) Rank();
        else Service.Log("FirebaseManager 로드 안됨");
    }

    private void Rank()
    {
        var user = FirebaseManager.nickName;

        for (int i = 0; i < user.Length; i++)
        {
            //본인과 이름이 같은가
            if (string.Equals(user[i], Json.GetName()))
            {
                //같을 경우 본인을 추월했나?
                NewRecord(i);
                return;
            }

            else if (NewRecord(i))
            {
                return;
            }
        }
    }

    private bool NewRecord(int _targetIndex)
    {
        var myPoint = Json.GetPlayPoint();
        var point = FirebaseManager.point;

        var length = point.Length - 1;
        var nextRankerIndex = _targetIndex - 1;
        var nextRankerPoint = nextRankerIndex < 0 ? myPoint + 1 : point[nextRankerIndex];

        //현재 랭커보다 점수가 높은지
        if (point[_targetIndex] < myPoint)
        {
            //내 바로 위 랭커보다는 점수가 낮을 경우에만 (중복 점수 등록 방지)
            if (myPoint < nextRankerPoint)
            {
                FirebaseManager.Add();
                FirebaseManager.Sort(_targetIndex);
            }

            return true;
        }

        return false;
    }
}
