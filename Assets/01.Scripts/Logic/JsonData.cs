using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class InventoryData
{
    public string block = "BasicBlock";
}

public class PlayerData
{
    public string nickName = "TestUser";
    public int advertPassCount;

    public int currentPoint;
    public int maxPoint;

    public float currentTime;
    public int combo;
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
    private static readonly JsonData data = JsonData.Load();

    /// <summary>
    /// 파일 세이브
    /// </summary>
    public static void Save() => data.Save();

    public static string GetBlock() => data.inventoryData.block;

    public static void SetMusicSound() => data.settingData.musicSound = !data.settingData.musicSound;
    public static void SetEffectSound() => data.settingData.effectSound = !data.settingData.effectSound;
    public static bool GetMusicSound() => data.settingData.musicSound;
    public static bool GetEffectSound() => data.settingData.effectSound;

    public static float GetPlayTime() => data.playerData.currentTime;
    public static void PlayTime(float _time) => data.playerData.currentTime = _time;

    public static int GetPlayPoint() => data.playerData.currentPoint;
    public static int GetPlayMaxPoint() => data.playerData.maxPoint;

    public static void SetAdvertPass(int _passCount) => data.playerData.advertPassCount = _passCount;
    public static int GetAdvertPass() => data.playerData.advertPassCount;

    public static int GetCombo() => data.playerData.combo;
    public static void SetCombo(int _combo) => data.playerData.combo = _combo;

    public static void SetName(string _name) => data.playerData.nickName = _name;
    public static string GetName() => data.playerData.nickName;

    public static void SetPlayPoint(int _point) => data.playerData.currentPoint = _point;

    public static void SaveMaxPoint()
    {
        var playerData = data.playerData;

        if (playerData.maxPoint < playerData.currentPoint)
        {
            data.playerData.maxPoint = playerData.currentPoint;
        }
    }
}
