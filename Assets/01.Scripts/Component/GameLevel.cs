using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    private List<(int, string)> sky = new();

#if UNITY_EDITOR
    private void Reset()
    {
        this.transform.position = Vector3.zero;
        this.name = typeof(GameLevel).Name;
    }
#endif

    private void Awake()
    {
        InitLevel();
        InitSky();
    }

    private void InitLevel()
    {
        //레벨 제한과 마테리얼 이름
        sky.Add((-1, "ResultSky"));
        sky.Add((100, "CloudyBrightDay"));
        sky.Add((200, "WarmSunriseGlow"));
        sky.Add((300, "CottonCandyMorning"));
        sky.Add((400, "VHSSkybox"));
        sky.Add((500, "OrangeSkybox"));
    }

    private void InitSky()
    {
        var playerLevel = Json.GetPlayMaxScore();

        for (int i = sky.Count - 1; -1 < i; i--)
        {
            var (level, skyName) = sky[i];

            if (level < playerLevel)
            {
                var path = Path.Combine("BackGround", skyName);
                var load = Resources.Load<Material>(path);
                var spawn = Instantiate(load);

                if (spawn) RenderSettings.skybox = spawn;
                break;
            }
        }
    }
}
