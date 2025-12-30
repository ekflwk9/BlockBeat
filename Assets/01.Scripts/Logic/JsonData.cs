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
    public int coin = 50;

    public string nickName = "Unnamed";
    public int advertPassCount;

    public int currentPoint;
    public int maxPoint;

    public float maxTime;
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

    public static bool GetBlockItem(Block.Name _blockName) => data.inventoryData.blocks.Contains(_blockName);
    public static void AddBlockItem(Block.Name _blockName) => data.inventoryData.blocks.Add(_blockName);

    public static Block.Name GetMainBlock() => data.inventoryData.currentBlock;
    public static void SetMainBlock(Block.Name _blockName) => data.inventoryData.currentBlock = _blockName;

    public static bool GetMusicSound() => data.settingData.musicSound;
    public static void SetMusicSound() => data.settingData.musicSound = !data.settingData.musicSound;

    public static bool GetEffectSound() => data.settingData.effectSound;
    public static void SetEffectSound() => data.settingData.effectSound = !data.settingData.effectSound;

    public static int GetPlayPoint() => data.playerData.currentPoint;
    public static int GetPlayMaxPoint() => data.playerData.maxPoint;

    public static void SetAdvertPass(int _passCount) => data.playerData.advertPassCount = _passCount;
    public static int GetAdvertPass() => data.playerData.advertPassCount;

    public static int GetCombo() => data.playerData.combo;
    public static void SetCombo(int _combo) => data.playerData.combo = _combo;

    public static void SetName(string _name) => data.playerData.nickName = _name;
    public static string GetName() => data.playerData.nickName;

    public static int GetCoin() => data.playerData.coin;
    public static void SetCoin(int _coin)
    {
        if (999999999 <= _coin) _coin = 999999999;
        else if (_coin < 0) _coin = 0;

        data.playerData.coin = _coin;
    }

    public static float GetMaxPlayTime() => data.playerData.maxTime;
    public static float GetPlayTime() => data.playerData.currentTime;
    public static void PlayTime(float _time)
    {
        var playerData = data.playerData;

        if (playerData.maxTime < _time) playerData.maxTime = _time;
        playerData.currentTime = _time;
    }

    public static void SetPlayPoint(int _point) => data.playerData.currentPoint = _point;
    public static void SaveMaxPoint()
    {
        var playerData = data.playerData;
        if (playerData.maxPoint < playerData.currentPoint) playerData.maxPoint = playerData.currentPoint;
    }
}
