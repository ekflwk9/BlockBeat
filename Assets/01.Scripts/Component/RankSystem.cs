using System.Collections.Generic;
using UnityEngine;

public class RankSystem : MonoBehaviour
{
    public const int maxCount = 15; //랭크 최대 표시 카운트

    private class ValueComparer : IComparer<KeyValuePair<string, int>>
    {
        public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
        {
            //높은 순으로 정렬
            if (x.Value < y.Value) return 1;
            else if (x.Value > y.Value) return -1;
            else return 0;
        }
    }

    private ValueComparer comparer = new();
    public bool newRecord { get; private set; }
    public List<KeyValuePair<string, int>> ranker { get; private set; }

    private void Awake()
    {
        if (FirebaseManager.connect) Rank();
        else Service.Log("FirebaseManager 로드 안됨");
    }

    private void Sort()
    {
        ranker = new(FirebaseManager.value);
        ranker.Sort(comparer);
    }

    private void Rank()
    {
        Sort();
        var length = ranker.Count < maxCount ? ranker.Count : maxCount;

        for (int i = 0; i < length; i++)
        {
            //본인과 이름이 같은가
            if (string.Equals(ranker[i].Key, Json.GetName()))
            {
                CheckNewRecord(i);
                return;
            }

            else if (CheckNewRecord(i))
            {
                return;
            }
        }
    }

    private bool CheckNewRecord(int _targetIndex)
    {
        var myPoint = Json.GetPlayMaxPoint();

        //현재 랭커보다 점수가 높은지
        if (ranker[_targetIndex].Value < myPoint)
        {
            var nextRankerIndex = _targetIndex - 1;
            var nextRankerPoint = nextRankerIndex < 0 ? myPoint + 1 : ranker[nextRankerIndex].Value;

            //내 바로 위 랭커보다는 점수가 낮을 경우에만 (중복 점수 등록 방지)
            if (myPoint < nextRankerPoint)
            {
                newRecord = true;
                Sort();
            }

            return true;
        }

        return false;
    }
}
