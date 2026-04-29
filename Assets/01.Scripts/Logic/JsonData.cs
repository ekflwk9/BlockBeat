using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryData
{
    public Block.Name currentBlock = Block.Name.Normal;
    public HashSet<Block.Name> blocks = new() { { Block.Name.Normal } };
}

public class PlayerData
{
    public const int MaxCoin = 999999999;
    public int coin = 50;

    public string nickName = Json.defaultName;
    public int advertPassCount;

    public int currentPoint;
    public int maxPoint;

    public float maxTime;
    public float currentTime;

    public int evade;
    public int maxEvade;
}

public class SettingData
{
    public bool musicSound = true;
    public bool effectSound = true;
}

public class JsonData
{
    private const string fileName = "S2c.Json";
    [JsonProperty] public PlayerData playerData { get; private set; } = new();
    [JsonProperty] public SettingData settingData { get; private set; } = new();
    [JsonProperty] public InventoryData inventoryData { get; private set; } = new();

    public void Save()
    {
#if UNITY_EDITOR
        var path = Path.Combine(Application.dataPath, "Editor", fileName);
#else
        var path = Path.Combine(Application.persistentDataPath, fileName);
#endif

        var saveFile = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(path, saveFile);
    }

    public static JsonData Load()
    {
#if UNITY_EDITOR
        var path = Path.Combine(Application.dataPath, "Editor", fileName);
#else
        var path = Path.Combine(Application.persistentDataPath, fileName);
#endif

        if (File.Exists(path))
        {
            var loadFile = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(loadFile))
            {
                return JsonConvert.DeserializeObject<JsonData>(loadFile);
            }
        }

        return new();
    }
}

public static class Json
{
    public const string defaultName = "Unnamed";
    private static readonly JsonData data = JsonData.Load();

    /// <summary>
    /// 파일 세이브
    /// </summary>
    public static void Save() => data.Save();

    /// <summary>
    /// 플레이어 정보에 관한 데이터
    /// </summary>
    /// <returns></returns>
    public static PlayerData PlayerData() => data.playerData;

    /// <summary>
    /// 게임 셋팅에 관한 데이터
    /// </summary>
    /// <returns></returns>
    public static SettingData SettingData() => data.settingData;

    /// <summary>
    /// 플레이어의 소지 아이템에 관한 데이터
    /// </summary>
    /// <returns></returns>
    public static InventoryData InventoryData() => data.inventoryData;
}
