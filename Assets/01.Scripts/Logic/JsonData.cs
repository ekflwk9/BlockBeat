using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class PlayerData
{
    public bool musicSound;
    public bool effectSound;
    public int currentScore;
    public int maxScore;
}

public class Data
{
    private const string fileName = "S2c.Json";
    [JsonProperty] public PlayerData playerData { get; private set; } = new();

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

    public static void MusicSound(bool _isOff) => data.playerData.musicSound = _isOff;
    public static void EffectSound(bool _isOff) => data.playerData.effectSound = _isOff;
    public static int GetPlayerScore() => data.playerData.currentScore;
    public static int GetPlayerMaxScore() => data.playerData.maxScore;

    public static void PlayerScore(int _score)
    {
        data.playerData.currentScore = _score;
        if (data.playerData.maxScore < _score) data.playerData.maxScore = _score;
    }

}
