using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private List<(int, string)> map = new();

#if UNITY_EDITOR
    private void Reset()
    {
        var newName = typeof(LevelSystem).Name;

        if (!string.Equals(newName, this.name))
        {
            this.name = newName;
            this.transform.position = Vector3.zero;
        }
    }
#endif

    private void Awake()
    {
        InitLevel();
        InitMap();
    }

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        Json.Save();
#endif
    }

    private void InitLevel()
    {
        //레벨 제한과 맵 프리팹 이름
        map.Add((-1, "Forest"));
        map.Add((50, "SnowMount"));
        map.Add((100, "Night"));
        map.Add((150, "Mars"));
    }

    private void InitMap()
    {
        var playerLevel = Json.GetPlayMaxPoint();

        for (int i = map.Count - 1; -1 < i; i--)
        {
            var (level, mapName) = map[i];

            if (level < playerLevel)
            {
                var path = Path.Combine("Map", mapName);
                var load = Resources.Load<GameObject>(path);
                var spawn = Instantiate(load);

                spawn.name = mapName;
                break;
            }
        }
    }

    /// <summary>
    /// 현재 레벨 반환
    /// </summary>
    /// <returns></returns>
    public int GetLevel()
    {
        var playerLevel = Json.GetPlayPoint();

        for (int i = map.Count - 1; -1 < i; i--)
        {
            var (level, mapName) = map[i];

            if (level < playerLevel) return i;
        }

        return map.Count - 1;
    }
}
