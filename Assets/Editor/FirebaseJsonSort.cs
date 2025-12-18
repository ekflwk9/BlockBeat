using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class FirebaseJsonSort
{
    private const string jsonName = "firebaseSort.json";

    private FirebaseJsonSort() { }

    [MenuItem("Tools/Firebase/Export Firebase JSON")]
    private static async void Export()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync();

        var reference = FirebaseDatabase.DefaultInstance.GetReference(FirebaseManager.referenceName);
        var snap = await reference.GetValueAsync();

        if (!snap.Exists)
        {
            Debug.Log("No data");
            return;
        }

        var json = snap.GetRawJsonValue();

        //Sort(json);
        Save(json);

        AssetDatabase.Refresh();
    }

    private static void Save(System.String _json)
    {
        var editorPath = Path.Combine(Application.dataPath, "Editor");
        var jsonPath = Path.Combine(editorPath, jsonName);

        if (!Directory.Exists(editorPath)) Directory.CreateDirectory(editorPath);
        File.WriteAllText(jsonPath, _json);
    }

    public static void Sort(System.String _json)
    {
        //id / name = string, point = int; 순서로 딕셔너리에 할당
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(_json);

        var list = new List<KeyValuePair<string, int>>();
        var e = data.GetEnumerator();

        while (e.MoveNext())
        {
            var uid = e.Current.Key;
            var value = e.Current.Value;

            if (value != null && value.ContainsKey("score"))
            {
                list.Add(new KeyValuePair<string, int>(uid, value["score"]));
            }
        }

        // score 내림차순 정렬
        list.Sort((a, b) => b.Value.CompareTo(a.Value));

        // 상위 10개 uid 보존
        var keep = new HashSet<string>();
        int limit = list.Count < 10 ? list.Count : 10;

        for (int i = 0; i < limit; i++)
        {
            keep.Add(list[i].Key);
        }

        // 나머지 제거
        var removeKeys = new List<string>();
        e = data.GetEnumerator();

        while (e.MoveNext())
        {
            if (!keep.Contains(e.Current.Key)) removeKeys.Add(e.Current.Key);
        }

        for (int i = 0; i < removeKeys.Count; i++)
        {
            data.Remove(removeKeys[i]);
        }

        _json = JsonConvert.SerializeObject(data, Formatting.None);
    }
}
#endif

