using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class FirebaseEditor
{
    private class FirebaseData
    {
        [JsonProperty] public Dictionary<string, UserInfo> leaderboard = new(); //FirebaseManager.referenceName
    }

    private class UserInfo
    {
        public readonly string nickname;
        public readonly int point;

        public UserInfo(string nickName, int point)
        {
            this.nickname = nickName;
            this.point = point;
        }
    }

    private class ValueComparer : IComparer<KeyValuePair<string, UserInfo>>
    {
        public int Compare(KeyValuePair<string, UserInfo> x, KeyValuePair<string, UserInfo> y)
        {
            //높은 순으로 정렬
            if (x.Value.point < y.Value.point) return 1;
            else if (x.Value.point > y.Value.point) return -1;
            else return 0;
        }
    }


    private const string jsonName = "firebaseDataSort.json";

    public static async void FirebaseSortExport()
    {
        try
        {
            await FirebaseApp.CheckAndFixDependenciesAsync();

            var reference = FirebaseDatabase.DefaultInstance.GetReference(FirebaseManager.referenceName);
            var snap = await reference.GetValueAsync();
            var firebaseData = snap.GetRawJsonValue();

            SortSave(firebaseData);
        }

        catch
        {
            Debug.Log("Firebase 데이터를 로드할 수 없음");
        }
    }

    private static void SortSave(System.String _firebaseData)
    {
        var infos = JsonConvert.DeserializeObject<Dictionary<string, UserInfo>>(_firebaseData);
        List<KeyValuePair<string, UserInfo>> tempPair = new(infos);
        tempPair.Sort(new ValueComparer());

        var data = new FirebaseData();
        var length = tempPair.Count < RankSystem.maxCount ? tempPair.Count : RankSystem.maxCount; //데이터 저장 제한

        for (int i = 0; i < length; i++)
        {
            data.leaderboard.Add(tempPair[i].Key, tempPair[i].Value);
        }

        _firebaseData = JsonConvert.SerializeObject(data, Formatting.None);
        Save(_firebaseData);
    }

    private static void Save(System.String _firebaseData)
    {
        var editorPath = Path.Combine(Application.dataPath, "Editor");
        var jsonPath = Path.Combine(editorPath, jsonName);

        if (!Directory.Exists(editorPath)) Directory.CreateDirectory(editorPath);
        File.WriteAllText(jsonPath, _firebaseData);

        AssetDatabase.Refresh();
    }
}
#endif

