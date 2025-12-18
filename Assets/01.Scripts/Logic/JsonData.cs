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

public class Data
{
    private const string fileName = "S2c.Json";
    [JsonProperty] public PlayerData playerData { get; private set; } = new();
    [JsonProperty] public SettingData settingData { get; private set; } = new();
    [JsonProperty] public InventoryData inventoryData { get; private set; } = new();

    public void Save()
    {
        var saveFile = JsonConvert.SerializeObject(this, Formatting.Indented);
        var path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, saveFile);
    }

    public static Data Load()
    {
        var file = Path.Combine(Application.dataPath, fileName);

        if (File.Exists(file))
        {
            var loadFile = File.ReadAllText(file);

            if (!string.IsNullOrEmpty(loadFile))
            {
                return JsonConvert.DeserializeObject<Data>(loadFile);
            }
        }

        return new();
    }
}

public static class Json
{
    private static readonly Data data = Data.Load();

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

    public static void PlayPoint(int _point)
    {
        data.playerData.currentPoint = _point;
        if (data.playerData.maxPoint < _point) data.playerData.maxPoint = _point;
    }
}
